using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.App_Start;
using log4net;
using Microsoft.Practices.Unity;

namespace Imobiliare.UI.Utils.HttpModules
{
    /// <summary>
    /// Src: http://www.hanselman.com/blog/AnIPAddressBlockingHttpModuleForASPNETIn9Minutes.aspx
    /// </summary>
    public class BlockIpAddressModule : IHttpModule
    {
        private EventHandler onBeginRequest;

        //static for caching
        private static List<BlockedIp> blockedIpAdresses;

        private static readonly ILog log = LogManager.GetLogger(typeof(BlockIpAddressModule));

        public BlockIpAddressModule()
        {
            onBeginRequest = new EventHandler(this.HandleBeginRequest);
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += onBeginRequest;
            if (blockedIpAdresses == null)
            {
                var container = UnityConfig.GetConfiguredContainer();
                blockedIpAdresses = container.Resolve<IUnitOfWork>().BlockedIpRepository.Find(x => x.Enabled);
            }
        }

        private void HandleBeginRequest(object sender, EventArgs evargs)
        {
            HttpApplication app = sender as HttpApplication;

            if (app != null)
            {
                string ipAddr = app.Context.Request.UserHostAddress;
                if (string.IsNullOrEmpty(ipAddr))
                {
                    return;
                }

                if (blockedIpAdresses != null)
                {
                    var item = blockedIpAdresses.FirstOrDefault(x => !x.IsRegex && x.Address == ipAddr);

                    if (item == null)
                    {
                        foreach (var ip in blockedIpAdresses.Where(x => x.IsRegex))
                        {
                            var found = Regex.Match(ipAddr, ip.Address);
                            if (found.Success)
                            {
                                item = ip;
                                break;
                            }
                        }
                    }

                    if (item != null)
                    {
                        var updateBlockCount = 0;
                        if (item.UpdateBlockCount)
                        {
                            var uow = UnityConfig.GetConfiguredContainer().Resolve<IUnitOfWork>();
                            updateBlockCount = uow.BlockedIpRepository.UpdateHitCount(item.Address);
                            uow.Complete();
                        }

                        if (item.TraceBlocking)
                        {
                            log.WarnFormat($"[BlockIpAddressModule] Blocked {ipAddr} to access site, updateBlockCount = {item.UpdateBlockCount}, " +
                                $"currentCount = {updateBlockCount}, regex = {item.IsRegex}, match pattern = {item.Address}");
                        }

                        //Return 404 - tell that it is not found
                        //Return 403 returns partial results apparently
                        app.Context.Response.StatusCode = 404;
                        app.Context.Response.SuppressContent = true;
                        app.Context.Response.End();
                    }
                }
            }
        }

        void IHttpModule.Dispose()
        {
            //DAPI Consider disposing here something?
        }
    }
}