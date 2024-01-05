using System;

using Imobiliare.Repositories.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Imobiliare.Entities;
using Imobiliare.ServiceLayer.Interfaces;
using Imobiliare.UI.Models;
using Logging;

namespace Imobiliare.UI.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    //[Route("api/[controller]")]
    //[ApiController]
    public class WebApiSuperAdminController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiSuperAdminController));
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailManagerService emailManagerService;
        private readonly IWebHostEnvironment hostingEnvironment;

        //Needed for web api parameterless ctor
        //public WebApiSuperAdminController() : base()
        //{
        //}

        public WebApiSuperAdminController(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService, IWebHostEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.emailManagerService = emailManagerService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("api/WebApiSuperAdmin/Salam")]
        public ActionResult Salam()
        {
            return Ok();
        }

        [HttpPost]
        //[ActionName("AnuntReactualizare")]
        [Route("api/WebApiSuperAdmin/AnuntReactualizare")]
        public ActionResult AnuntReactualizare(AnuntDto anuntDto)
        {
            unitOfWork.AnunturiRepository.ReActualizareAnunt(anuntDto.AnuntId);
            unitOfWork.AnunturiRepository.AddCustomNoteToImobil(anuntDto.AnuntId, User.Identity.Name + " react(" + DateTime.Now.ToShortDateString() + ")");
            unitOfWork.Complete();
            log.Debug($"WEBAPI Admin {User.Identity.Name} renewed imobil with id {anuntDto.AnuntId}");
            return Ok();
        }

        [HttpPost]
        [Route("api/WebApiSuperAdmin/AnuntAprobat")]
        public ActionResult AnuntAprobat(AnuntDto anuntDto)
        {
            var imobil = unitOfWork.AnunturiRepository.Single(x => x.Id == anuntDto.AnuntId);
            var previousState = imobil.StareAprobare;
            imobil.StareAprobare = StareAprobare.Aprobat;

            imobil.DataAprobare = DateTime.Now;
            imobil.DataAdaugare = DateTime.Now;

            //Send email confirmation ca s-a aprobat
            //Only to users which are not administrators
            //Only if previous state was "InAsteptare"
            var user = unitOfWork.UsersRepository.GetUserProfileById(imobil.UserId, false);
            if (previousState == StareAprobare.InAsteptare && user.Role != Role.Administrator)
            {
                emailManagerService.AnuntAprobatRelatedEmail(
                  imobil.Title,
                  anuntDto.AnuntId,
                  user.Id,
                  user.UserName);
            }
            else if (user.Role != Role.Administrator)
            {
                log.Debug($"Amin approved imobil {imobil.Title} with id {imobil.Id}");
            }
            else if (previousState != StareAprobare.InAsteptare)
            {
                log.Debug($"Amin approved imobil {imobil.Title} with id {imobil.Id}, no email send needed because was not InAsteptare before");
            }
            unitOfWork.Complete();
            log.Debug($"Amin changed state of imobil {imobil.Title} with id {imobil.Id} to state {StareAprobare.Aprobat}");
            return Ok();
        }

        [HttpPost]
        [Route("api/WebApiSuperAdmin/AnuntStergere")]
        public ActionResult AnuntStergere(AnuntDto anuntDto)
        {
            unitOfWork.AnunturiRepository.DeleteImobil(anuntDto.AnuntId, hostingEnvironment.WebRootPath);
            unitOfWork.Complete();
            log.Debug($"Deleted imobil with id {anuntDto.AnuntId} by administrator {User.Identity.Name}");
            return Ok();
        }
    }
}