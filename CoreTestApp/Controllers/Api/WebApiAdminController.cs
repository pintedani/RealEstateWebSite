using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Imobiliare.UI.Models;
using Logging;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers.Api
{
    public class WebApiAdminController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiAdminController));
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment hostingEnvironment;

        public WebApiAdminController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("api/WebApiAdmin/AnuntStergere")]
        public ActionResult AnuntStergere(AnuntDto anuntDto)
        {
            this.CheckRights(anuntDto.AnuntId);
            this.unitOfWork.AnunturiRepository.DeleteImobil(anuntDto.AnuntId, hostingEnvironment.WebRootPath);
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Deleted imobil with id {anuntDto.AnuntId}", userName: User.Identity.Name);
            this.unitOfWork.Complete();
            return this.Ok();
        }

        /// <summary>
        /// Check rights that no other user has access to modify other imobil states except Admin
        /// </summary>
        /// <param name="anuntId"></param>
        private void CheckRights(int anuntId)
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            if (user.Role == Role.Administrator)
            {
                return;
            }
            var anunt = this.unitOfWork.AnunturiRepository.Single(x => x.Id == anuntId);
            if (anunt.UserProfile.UserName != user.UserName)
            {
                throw new Exception("Different user(!=Administrator)" + user.UserName + " tried to change state of other anunt" + anuntId);
            }
        }
    }
}