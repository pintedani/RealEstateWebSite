﻿using Imobiliare.Entities;
using Imobiliare.Managers.ExtensionMethods;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Imobiliare.UI.BusinessLayer;
using Imobiliare.UI.Models;
using Imobiliare.UI.ViewModels;
using Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.RegularExpressions;

namespace Imobiliare.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailManagerService emailManagerService;
        private readonly IRecaptchaValidator recaptchaValidator;

        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public HomeController(
          IUnitOfWork unitOfWork,
          IEmailManagerService emailManagerService,
          IRecaptchaValidator recaptchaValidator,
          RoleManager<IdentityRole> roleManager,
          UserManager<UserProfile> userManager)
        {
            this.emailManagerService = emailManagerService;
            this.recaptchaValidator = recaptchaValidator;

            this.unitOfWork = unitOfWork;

            //if (!roleManager.RoleExistsAsync("Admin").Result)
            //{
                //var adminRole = new IdentityRole("Admin");
                //roleManager.CreateAsync(adminRole).Wait();

                //var userProfile = this.unitOfWork.UsersRepository.GetUserProfileById("1", false);
                //var role = userProfile.Role;
                //userManager.AddToRoleAsync(userProfile, "Admin");
            //}
        }

        public ActionResult Index()
        {
            //https://localhost:7034/Anunturi/ApartamentDetalii?imobilid=27765&titlu=sfd
            //var userProfile = this.unitOfWork.UsersRepository.GetUserProfileById("1", false);
            //var role = userProfile.Role;
            //userManager.AddToRoleAsync(user, "Admin")

            ViewBag.TipOferta = TipProprietate.Toate.ToString();

            ViewBag.TotalNumberAnunturiPeJudete = this.unitOfWork.AnunturiRepository.GetTotalNumarAnunturiPerJudete();
            ViewBag.Judete = this.unitOfWork.JudetRepository.Judete();
            ViewBag.UltimeleStiri = this.unitOfWork.StiriRepository.GetLastAddedStiri();

            ViewBag.LastAddedImobils = this.unitOfWork.AnunturiRepository.GetLastAddedImobils(4);
            ViewBag.LastAddedImobilsCautare = this.unitOfWork.AnunturiRepository.GetLastAddedAnunturiCautare(3);

            return View();
        }

        public ActionResult IndexCoreDefault()
        {
            return View();
        }

        public ActionResult IndexOld(string tipOferta)
        {
            ViewBag.TipOferta = TipProprietate.Toate.ToString();
            if (tipOferta != null)
            {
                ViewBag.TipOferta = tipOferta;
            }

            ViewBag.TotalNumberAnunturiPeJudete = this.unitOfWork.AnunturiRepository.GetTotalNumarAnunturiPerJudete();
            ViewBag.Judete = this.unitOfWork.JudetRepository.Judete();
            ViewBag.UltimeleStiri = this.unitOfWork.StiriRepository.GetLastAddedStiri();

            ViewBag.LastAddedImobils = this.unitOfWork.AnunturiRepository.GetLastAddedImobils(4);
            ViewBag.LastAddedImobilsCautare = this.unitOfWork.AnunturiRepository.GetLastAddedAnunturiCautare(3);

            return View();
        }

        //Only for redirecting to new route: /Anunturi/AnunturiUtilizator
        public ActionResult AnunturiUtilizator(string userId)
        {
            return RedirectToActionPermanent("AnunturiUtilizator", "Anunturi", new { userId = userId });
        }

        [HttpGet]
        public ActionResult SelectJudet(string judetName)
        {
            //Remove spaces from judet Name
            var judet = this.unitOfWork.JudetRepository.Judete().FirstOrDefault(x => x.Nume == judetName.Replace(" ", ""));
            return RedirectToAction(
              "AnunturiList",
              "Anunturi",
              new
              {
                  judetName = judet.Nume,
                  filters = "tip-" + TipOfertaDisplayFormatter.GetDisplayStringForUrl(TipProprietate.Toate, TipOfertaGen.Toate)
              });
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Informatii de contact administrator site.";

            return View();
        }

        [Route("Despre-Cookies")]
        public ActionResult DespreCookies()
        {
            ViewBag.Message = "Politica de utilizare a Cookie-urilor pe apartamente24.ro.";

            return View();
        }

        public ActionResult TermenisiConditii()
        {
            ViewBag.Message = "Termeni si conditii.";

            return View();
        }

        public ActionResult TermenisiConditiiB5()
        {
            ViewBag.Message = "Termeni si conditii.";

            return View();
        }

        public ActionResult Despre()
        {
            return View();
        }

        public ActionResult Orase(int judetId)
        {
            if (judetId != 0)
            {
                return Json(this.unitOfWork.OrasRepository.GetSelectableOrases(judetId));
            }
            return this.Json(new List<Oras>());
        }

        public ActionResult Cartiere(int orasId)
        {
            if (orasId != 0)
            {
                var result = Json(this.unitOfWork.CartierRepository.GetSelectableCartiers(orasId));
                return result;
            }
            return this.Json(new List<Cartier>());
        }

        [HttpGet]
        public ActionResult LocationSearchResults(string searchText, bool includingTotJudet)
        {
            if (!string.IsNullOrEmpty(searchText) && searchText.Length > 0)
            {
                Dictionary<int, string> locations = this.unitOfWork.OrasRepository.GetMatchingLocations(searchText, includingTotJudet);

                var jsonList = new List<string>();
                foreach (var location in locations)
                {
                    jsonList.Add(location.Key + "|" + location.Value);
                }

                return Json(jsonList);
            }
            return this.Json(new List<string>());
        }

        public ActionResult RetrieveTelefon(string anuntId)
        {
            if (anuntId != string.Empty && anuntId.Length > 0)
            {
                string telefon = this.unitOfWork.AnunturiRepository.GetTelefonForAnunt(anuntId);
                return Json(telefon);
            }
            return this.Json(string.Empty);
        }

        public JsonResult GetSavedAnunturiFromCookie(List<string> savedIds)
        {
            var anunturi = new List<string>();

            if (savedIds != null)
            {
                var imobils = new List<Imobil>();
                foreach (var savedId in savedIds)
                {
                    //returns null if imobil not exists
                    var parsedId = savedId.ParseToInt();
                    var imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == parsedId);
                    imobils.Add(imobil);
                }

                //If imobil exists in cookie but deleted fizically, just not return it
                foreach (var imobil in imobils.Where(x => x != null))
                {
                    string poza = $"uploadedPhotos/{imobil.FirstPhoto}";
                    anunturi.Add(imobil.Id + "|" + imobil.Title + "|" + imobil.Judet.Nume + "|" + imobil.Oras.Nume + "|" + poza);
                }
            }
            return this.Json(anunturi);
        }

        //Any time a filter is changed on getfilteredanunturi screen
        [HttpPost]
        public ActionResult GetNumarAnunturiForFilter(ImobilFilter filter)
        {
            int totalNumber = 0;
            filter.StareAprobare = StareAprobare.Aprobat;
            this.unitOfWork.AnunturiRepository.GetAllImobil(filter, out totalNumber);

            return this.Content(totalNumber.ToString());
        }

        [HttpPost]
        public ActionResult GetNumarAnunturiForFilterHomeSearch(ImobilFilter filter, string localitateString)
        {
            filter.StareAprobare = StareAprobare.Aprobat;

            if (!string.IsNullOrEmpty(localitateString))
            {
                var judete = this.unitOfWork.JudetRepository.Judete();
                if (localitateString.IndexOf("(tot judetul)", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    string judetWithSpaceAfter = localitateString.Split(new char[] { '(' }, 2)[0];
                    string numeJudet = judetWithSpaceAfter.Remove(judetWithSpaceAfter.Length - 1);
                    if (judete.Find(x => x.Nume == numeJudet) != null)
                    {
                        filter.JudetId = judete.Single(x => x.Nume == numeJudet).Id;
                    }
                }
                else
                {
                    var searchValues = localitateString.Split('|');
                    if (searchValues.Count() == 2)
                    {
                        var selectedJudet = judete.FirstOrDefault(x => x.PrescurtareAuto == searchValues[1].Trim(' '));
                        if (selectedJudet != null)
                        {
                            var oras =
                              this.unitOfWork.OrasRepository.GetSelectableOrases(selectedJudet.Id)
                                .FirstOrDefault(x => x.Nume == searchValues[0].TrimEnd(' '));
                            if (oras != null)
                            {
                                filter.OrasId = oras.Id;
                            }
                        }
                    }
                }
            }
            int totalNumber = 0;
            this.unitOfWork.AnunturiRepository.GetAllImobil(filter, out totalNumber);

            return this.Content(totalNumber.ToString());
        }

        //Numar anunturi on Getfilteredanunturi screen when dropdown is clicked
        [HttpPost]
        public ActionResult GetNumarAnunturiPerTipOfertaForFilter(ImobilFilter filter, string localitateString)
        {
            ConstructImobilFilter(filter, localitateString);

            Dictionary<TipProprietate, int> situationForTipOferta = this.unitOfWork.AnunturiRepository.GetSituationForTipOferta(filter);

            string returnString = string.Empty;
            foreach (var situation in situationForTipOferta)
            {
                returnString += situation.Key + "," + situation.Value + "|";
            }

            var returnedJson = this.Json(returnString);

            return returnedJson;
        }

        [HttpPost]
        public ActionResult GetNumarAnunturiPerTipOfertaGenForFilter(ImobilFilter filter, string localitateString)
        {
            ConstructImobilFilter(filter, localitateString);

            Dictionary<TipOfertaGen, int> situationForTipOferta = this.unitOfWork.AnunturiRepository.GetSituationForTipProprietate(filter);

            string returnString = string.Empty;
            foreach (var situation in situationForTipOferta)
            {
                returnString += situation.Key + "," + situation.Value + "|";
            }

            var returnedJson = this.Json(returnString);

            return returnedJson;
        }

        private void ConstructImobilFilter(ImobilFilter filter, string localitateString)
        {
            filter.StareAprobare = StareAprobare.Aprobat;

            if (!string.IsNullOrEmpty(localitateString))
            {
                var judete = this.unitOfWork.JudetRepository.Judete();
                if (localitateString.IndexOf("(tot judetul)", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    string judetWithSpaceAfter = localitateString.Split(new char[] { '(' }, 2)[0];
                    string numeJudet = judetWithSpaceAfter.Remove(judetWithSpaceAfter.Length - 1);
                    if (judete.Find(x => x.Nume == numeJudet) != null)
                    {
                        filter.JudetId = judete.Single(x => x.Nume == numeJudet).Id;
                    }
                }
                else
                {
                    var searchValues = localitateString.Split('|');
                    if (searchValues.Count() == 2)
                    {
                        var selectedJudet = judete.FirstOrDefault(x => x.PrescurtareAuto == searchValues[1].Trim(' '));
                        if (selectedJudet != null)
                        {
                            var oras =
                              this.unitOfWork.OrasRepository.GetSelectableOrases(selectedJudet.Id)
                                .FirstOrDefault(x => x.Nume == searchValues[0].TrimEnd(' '));
                            if (oras != null)
                            {
                                filter.OrasId = oras.Id;
                            }
                        }
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult TrimiteMesajContact()
        {
            if (this.unitOfWork.SystemSettingsRepository.SystemSettings.CapchaEnabled)
            {
                string response = Request.Form["g-recaptcha-response"];
                //string response = Request["g-recaptcha-response"];

                CaptchaResponse captchaResponse = this.recaptchaValidator.GetCaptchaResponse(response);
                if (!captchaResponse.Success)
                {
                    return Json(new { success = false, message = "Va rugam confirmati ca nu sunteti robot!" });
                }
            }
            var emailContact = Request.Form["EmailContact"];
            var telefonContact = Request.Form["TelefonContact"];
            var mesajContact = Request.Form["MesajContact"];

            if (!Regex.IsMatch(emailContact, "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$"))
            {
                return Json(new { success = false, message = "Emailul introdus este invalid!" });

            }
            if (string.IsNullOrWhiteSpace(telefonContact) || telefonContact == "Telefonul tau")
            {
                return Json(new { success = false, message = "Telefonul introdus este invalid!" });
            }
            if (string.IsNullOrWhiteSpace(mesajContact) || mesajContact == string.Empty)
            {
                return Json(new { success = false, message = "Va rugam introduceti un mesaj!" });
            }

            this.emailManagerService.NotifyAdminContactForm(telefonContact, emailContact, mesajContact);

            return Json(new { success = true, message = "Mesajul a fost receptionat. Va multumim!" });
        }

        public ActionResult TrimiteMesajAbuzAnunt()
        {
            if (this.unitOfWork.SystemSettingsRepository.SystemSettings.CapchaEnabled)
            {
                string response = Request.Form["g-recaptcha-response"];
                //var response = Request["g-recaptcha-response"];

                CaptchaResponse captchaResponse = this.recaptchaValidator.GetCaptchaResponse(response);
                if (!captchaResponse.Success)
                {
                    //Eroare este cuvantul cheie ca sa functioneze
                    return Content("Eroare: Va rugam confirmati ca nu sunteti robot");
                }
            }
            var mesajAbuz = Request.Form["MesajAbuzCumparator"];
            var emailContact = Request.Form["EmailContact"];
            var idAnunt = Request.Form["IdAnuntVanzator"];
            var titluAnunt = Request.Form["TitluAnunt"];
            var selectAbuzTip = Request.Form["SelectAbuzRadioButtons"];

            if (!Regex.IsMatch(emailContact, "^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$"))
            {
                return Content("Eroare: Emailul introdus este invalid");
            }

            if (mesajAbuz == string.Empty)
            {
                return Content("Eroare: Va rugam introduceti un mesaj");
            }

            this.emailManagerService.NotifyAdminRaportAbuzAnunt(idAnunt.ToString().ParseToInt(), mesajAbuz, selectAbuzTip, emailContact, titluAnunt);

            var mesaj = "Mesajul a fost receptionat si va fi analizat. Va multumim.";
            return Content(mesaj);
        }
    }
}
