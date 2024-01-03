//using System.Web.Http;
//using Imobiliare.Entities;
//using Imobiliare.ServiceLayer.Interfaces;
//using Imobiliare.UI.Models;

//namespace Imobiliare.UI.Controllers
//{
//    using System;
//    using System.Web.Mvc;

//    using Imobiliare.Repositories.Interfaces;

//    using log4net;

//    [Authorize(Roles = "Admin")]
//    public class WebApiSuperAdminController : ApiController
//    {
//        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiSuperAdminController));
//        private readonly IUnitOfWork unitOfWork;
//        private readonly IEmailManagerService emailManagerService;

//        //Needed for web api parameterless ctor
//        public WebApiSuperAdminController() : base()
//        {
//        }

//        public WebApiSuperAdminController(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService)
//        {
//            this.unitOfWork = unitOfWork;
//            this.emailManagerService = emailManagerService;
//        }

//        [HttpPost]
//        [ActionName("AnuntReactualizare")]
//        public IHttpActionResult AnuntReactualizare(AnuntDto anuntDto)
//        {
//            this.unitOfWork.AnunturiRepository.ReActualizareAnunt(anuntDto.AnuntId);
//            this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(anuntDto.AnuntId, User.Identity.Name + " react(" + DateTime.Now.ToShortDateString() + ")");
//            this.unitOfWork.Complete();
//            log.Debug($"WEBAPI Admin {0} renewed imobil with id {1}", User.Identity.Name, anuntDto.AnuntId);
//            return this.Ok();
//        }

//        [HttpPost]
//        [ActionName("AnuntAprobat")]
//        public IHttpActionResult AnuntAprobat(AnuntDto anuntDto)
//        {
//            var imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == anuntDto.AnuntId);
//            var previousState = imobil.StareAprobare;
//            imobil.StareAprobare = StareAprobare.Aprobat;

//            imobil.DataAprobare = DateTime.Now;
//            imobil.DataAdaugare = DateTime.Now;

//            //Send email confirmation ca s-a aprobat
//            //Only to users which are not administrators
//            //Only if previous state was "InAsteptare"
//            var user = this.unitOfWork.UsersRepository.GetUserProfileById(imobil.UserId, false);
//            if (previousState == StareAprobare.InAsteptare && user.Role != Role.Administrator)
//            {
//                this.emailManagerService.AnuntAprobatRelatedEmail(
//                  imobil.Title,
//                  anuntDto.AnuntId,
//                  user.Id,
//                  user.UserName);
//            }
//            else if (user.Role != Role.Administrator)
//            {
//                log.Debug($"Amin approved imobil {0} with id {1}, no email send needed because added by admin", imobil.Title, imobil.Id);
//            }
//            else if (previousState != StareAprobare.InAsteptare)
//            {
//                log.Debug($"Amin approved imobil {0} with id {1}, no email send needed because was not InAsteptare before", imobil.Title, imobil.Id);
//            }
//            this.unitOfWork.Complete();
//            log.Debug($"Amin changed state of imobil {0} with id {1} to state {2}", imobil.Title, imobil.Id, StareAprobare.Aprobat);
//            return this.Ok();
//        }

//        [HttpPost]
//        [ActionName("AnuntStergere")]
//        public IHttpActionResult AnuntStergere(AnuntDto anuntDto)
//        {
//            this.unitOfWork.AnunturiRepository.DeleteImobil(anuntDto.AnuntId);
//            this.unitOfWork.Complete();
//            log.Debug($"Deleted imobil with id {0} by administrator {1}", anuntDto.AnuntId, User.Identity.Name);
//            return this.Ok();
//        }
//    }
//}