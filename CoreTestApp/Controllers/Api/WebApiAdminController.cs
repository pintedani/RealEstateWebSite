//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Imobiliare.UI.Controllers
//{
//    using System.Web.Http;

//    using Imobiliare.Entities;
//    using Imobiliare.Repositories.Interfaces;
//    using Imobiliare.ServiceLayer.Interfaces;
//    using Imobiliare.UI.Models;

//    using log4net;

//    public class WebApiAdminController : ApiController
//    {
//        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiSuperAdminController));
//        private readonly IUnitOfWork unitOfWork;

//        //Needed for web api parameterless ctor
//        public WebApiAdminController() : base()
//        {

//        }

//        public WebApiAdminController(IUnitOfWork unitOfWork)
//        {
//            this.unitOfWork = unitOfWork;
//        }

//        [HttpPost]
//        [ActionName("AnuntStergere")]
//        public IHttpActionResult AnuntStergere(AnuntDto anuntDto)
//        {
//            this.CheckRights(anuntDto.AnuntId);
//            this.unitOfWork.AnunturiRepository.DeleteImobil(anuntDto.AnuntId);
//            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Deleted imobil with id {anuntDto.AnuntId}", userName: User.Identity.Name);
//            this.unitOfWork.Complete();
//            return this.Ok();
//        }

//        /// <summary>
//        /// Check rights that no other user has access to modify other imobil states except Admin
//        /// </summary>
//        /// <param name="anuntId"></param>
//        private void CheckRights(int anuntId)
//        {
//            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
//            if (user.Role == Role.Administrator)
//            {
//                return;
//            }
//            var anunt = this.unitOfWork.AnunturiRepository.Single(x => x.Id == anuntId);
//            if (anunt.UserProfile.UserName != user.UserName)
//            {
//                throw new Exception("Different user(!=Administrator)" + user.UserName + " tried to change state of other anunt" + anuntId);
//            }
//        }
//    }
//}