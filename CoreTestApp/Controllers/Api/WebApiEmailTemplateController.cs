﻿using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Imobiliare.UI.Models.DTOs;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Logging;

namespace Imobiliare.UI.Controllers.Api
{
    public class WebApiEmailTemplateController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiEmailTemplateController));
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailManagerService emailManagerService;
        private readonly IRecaptchaValidator recaptchaValidator;

        //Needed for web api parameterless ctor
        //public WebApiEmailTemplateController() : base()
        //{

        //}

        public WebApiEmailTemplateController(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService, IRecaptchaValidator recaptchaValidator)
        {
            this.unitOfWork = unitOfWork;
            this.emailManagerService = emailManagerService;
            this.recaptchaValidator = recaptchaValidator;
        }

        [HttpPost]
        [ActionName("TrimiteMesajGeneralAgent")]
        //TODO check if Mesaj Property is displayed on UI
        public ActionResult TrimiteMesajGeneralAgent(MesajContactAgent mesajContactAgent)
        {
            if (mesajContactAgent == null)
            {
                return this.BadRequest();
            }

            if (this.unitOfWork.SystemSettingsRepository.SystemSettings.CapchaEnabled)
            {
                CaptchaResponse captchaResponse = this.recaptchaValidator.GetCaptchaResponse(mesajContactAgent.Recaptcha);
                if (!captchaResponse.Success)
                {
                    log.Error($"Eroare: Va rugam confirmati ca nu sunteti robot" + mesajContactAgent.Recaptcha);
                    //TODO check if Mesaj Property is displayed on UI
                    return this.BadRequest("Eroare: Va rugam confirmati ca nu sunteti robot");
                    //return Content(HttpStatusCode.BadRequest, new { Mesaj = "Eroare: Va rugam confirmati ca nu sunteti robot" });
                }
            }

            //TODO DAPI: Whats with this sleep?
            Thread.Sleep(1000);

            if (mesajContactAgent.EmailCumparator == null ||
              !Regex.IsMatch(mesajContactAgent.EmailCumparator, "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$"))
            {
                return this.BadRequest("Eroare: Emailul introdus este invalid.");
                //return Content(HttpStatusCode.BadRequest, new { Mesaj = "Eroare: Emailul introdus este invalid." });
            }
            if (mesajContactAgent.TelefonCumparator == null || mesajContactAgent.TelefonCumparator == "Telefonul tau")
            {
                return this.BadRequest("Eroare: Telefonul introdus este invalid.");
                //return Content(HttpStatusCode.BadRequest, new { Mesaj = "Eroare: Telefonul introdus este invalid." });
            }
            if (string.IsNullOrEmpty(mesajContactAgent.Mesaj))
            {
                return this.BadRequest("Eroare: Va rugam introduceti un mesaj.");
                //return Content(HttpStatusCode.BadRequest, new { Mesaj = "Eroare: Va rugam introduceti un mesaj." });
            }

            this.emailManagerService.SendAgentImobiliarContactEmail(mesajContactAgent.EmailAgent, mesajContactAgent.Mesaj, mesajContactAgent.EmailCumparator, mesajContactAgent.TelefonCumparator);

            //Todo DAPI - check if needed
            var mesaj = "Un email a fost trimis la adresa de email a vanzatorului cu mesajul dumneavoastra.";

            return this.Ok(mesaj);
        }
    }
}