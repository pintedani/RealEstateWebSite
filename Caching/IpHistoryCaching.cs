using Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Caching
{
    public static class IpHistoryCaching
    {
        private const int MaxCacheNumber = 100;
        private static readonly ILog log = LogManager.GetLogger(typeof(IpHistoryCaching));

        private static List<IPHistory> iPHistories = new List<IPHistory>();

        public static bool ShouldLogFrequentAccesor(string ip)
        {
            var result = true;
            var cachedIpHistory = iPHistories.FirstOrDefault(x => x.Ip == ip);

            if (cachedIpHistory != null)
            {
                if ((DateTime.Now - cachedIpHistory.LastAccess).Seconds < 3)
                {
                    result = false;
                }
            }

            UpdateIpHistory(ip);

            return result;
        }

        private static void UpdateIpHistory(string ip)
        {
            if (iPHistories.Count > MaxCacheNumber)
            {
                log.Debug($"Refreshing IpHistoryCaching, max cached ips {MaxCacheNumber}");
                iPHistories = new List<IPHistory>();
            }

            var ipHistory = iPHistories.FirstOrDefault(x => x.Ip == ip);
            if (ipHistory != null)
            {
                ipHistory.AccesTimes += 1;
                ipHistory.LastAccess = DateTime.Now;
            }
            else
            {
                iPHistories.Add(new IPHistory
                {
                    Ip = ip,
                    AccesTimes = 1,
                    LastAccess = DateTime.Now
                });
            }
        }
    }
}
