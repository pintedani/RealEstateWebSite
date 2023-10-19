using Caching;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models.HelperModels;
using Imobiliare.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using Imobiliare.Managers.ExtensionMethods;
using Imobiliare.UI.Utils.UrlSeoFormatters;
using Imobiliare.UI.BusinessLayer;
using Imobiliare.UI.Utils;
using Logging;
using Microsoft.AspNetCore.Http;

namespace Imobiliare.UI.Controllers
{
    public class AnunturiController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private static readonly ILog log = LogManager.GetLogger(typeof(AnunturiController));
        private HttpContext httpContext;

        public AnunturiController(IUnitOfWork unitOfWork/*, HttpContext httpContext*/)
        {
            this.unitOfWork = unitOfWork;
            //this.httpContext = httpContext;
        }
        public ActionResult ApartamentdetaliiIT()
        {

            return View();
        }

        [HttpGet]
        public ActionResult SearchForValue(string searchValue, string tipProprietate, string tipOfertaGen, string pretMin, string pretMax, string camereMin, string camereMax, string mpMin, string mpMax, string page, string mapMode, string cartier, string vanzator)
        {
            //ToDo: Check if tipOfeta1 = null(because of many log exceptions where this was null on parsing the enum)
            //In normal case should not happen
            if (tipProprietate == null || tipOfertaGen == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
                //DAPI: logs were too many, so no need to log
                //log.WarnFormat("SearchForValue received null value for tipoferta, searchvalue: {0}", searchValue);
            }

            var tipProprietateEnum = tipProprietate.EnumParse<TipProprietate>();
            var tipOfertaEnum = tipOfertaGen.EnumParse<TipOfertaGen>();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return RedirectToAction(nameof(AnunturiList), new { filters = GenerateAnunturiFilter(null, 0, tipProprietateEnum, tipOfertaEnum, pretMin, pretMax, camereMin, camereMax, mpMin, mpMax, page, mapMode, cartier, vanzator) });
            }

            if (Regex.IsMatch(searchValue, "^([1-9][0-9]{0,7})$"))
            {
                var id = int.Parse(searchValue);
                var anunt = this.unitOfWork.AnunturiRepository.SingleOrDefault(x => x.Id == id);
                if (anunt != null)
                {
                    return RedirectToAction(nameof(ApartamentDetalii), new { imobilId = anunt.Id, titlu = "" });
                }
                else
                {
                    TempData[TempDataSeverity.WarningMessage.ToString()] = "Anunt Id nu a fost gasit";
                    return RedirectToAction(nameof(SugestiiCautare), new { text = searchValue });
                }
            }

            var judete = this.unitOfWork.JudetRepository.Judete();

            //TODO DAPI Fix this case
            if (searchValue.IndexOf("(tot judetul)", StringComparison.OrdinalIgnoreCase) != -1)
            {
                string judetWithSpaceAfter = searchValue.Split(new char[] { '(' }, 2)[0];
                string numeJudet = judetWithSpaceAfter.Remove(judetWithSpaceAfter.Length - 1);
                var judet = judete.Find(x => x.Nume == numeJudet);
                if (judet != null)
                {
                    return RedirectToAction(nameof(AnunturiList), new { filters = GenerateAnunturiFilter(null, judet.Id, tipProprietateEnum, tipOfertaEnum, pretMin, pretMax, camereMin, camereMax, mpMin, mpMax, page, mapMode, cartier, vanzator) });
                }
                else
                {
                    log.WarnFormat("Some user searched for unknown value. No judet found for search value: {0} and tipoferta {1}", searchValue, tipProprietate);
                    return RedirectToAction(nameof(SugestiiCautare), new { text = searchValue });
                }
            }

            var searchValues = searchValue.Split('|');
            if (searchValues.Count() < 2)
            {
                Oras oras = this.unitOfWork.OrasRepository.GetUniqueSelectableOras(searchValues[0]);
                if (oras != null)
                {
                    return RedirectToAction(nameof(AnunturiList), new { filters = GenerateAnunturiFilter(oras, oras.JudetId.Value, tipProprietateEnum, tipOfertaEnum, pretMin, pretMax, camereMin, camereMax, mpMin, mpMax, page, mapMode, cartier, vanzator) });
                }
                else
                {
                    log.DebugFormat("Some user searched for unknown value. No localitate found for search value: {0} and tipoferta {1}", searchValue, tipProprietate);
                    return RedirectToAction(nameof(SugestiiCautare), new { text = searchValue });
                }
            }

            //This is the normal case
            var selectedJudet = judete.FirstOrDefault(x => x.PrescurtareAuto == searchValues[1].Trim(' '));
            if (selectedJudet != null)
            {
                var oras = this.unitOfWork.OrasRepository.GetSelectableOrases(selectedJudet.Id).FirstOrDefault(x => x.Nume == searchValues[0].TrimEnd(' '));
                if (oras != null)
                {
                    return RedirectToAction(nameof(AnunturiList), new { filters = GenerateAnunturiFilter(oras, oras.JudetId.Value, tipProprietateEnum, tipOfertaEnum, pretMin, pretMax, camereMin, camereMax, mpMin, mpMax, page, mapMode, cartier, vanzator) });
                }
            }

            //TODO Show search suggestions
            log.DebugFormat("Some user searched for unknown value. No localitate found for search value: {0} and tipoferta {1}", searchValue, tipProprietate);
            return RedirectToAction(nameof(SugestiiCautare), new { text = searchValue });
        }

        private string GenerateAnunturiFilter(Oras oras, int judetId, TipProprietate tipProprietate, TipOfertaGen tipOferta, string pretMin, string pretMax, string camereMin, string camereMax, string mpMin, string mpMax, string page, string mapMode, string cartier, string vanzator)
        {
            var imobilFilter = new ImobilFilter();
            imobilFilter.TipOfertaGen = tipOferta;
            imobilFilter.TipProprietate = tipProprietate;

            if (judetId != 0)
            {
                imobilFilter.JudetId = judetId;
            }

            if (oras != null)
            {
                imobilFilter.OrasId = oras.Id;
            }

            if (pretMin != null)
            {
                imobilFilter.PretMin = int.Parse(pretMin);
            }

            if (pretMax != null)
            {
                imobilFilter.PretMax = int.Parse(pretMax);
            }

            if (camereMin != null)
            {
                imobilFilter.CamereMin = int.Parse(camereMin);
            }

            if (camereMax != null)
            {
                imobilFilter.CamereMax = int.Parse(camereMax);
            }

            if (mpMin != null)
            {
                imobilFilter.MpMin = int.Parse(mpMin);
            }

            if (mpMax != null)
            {
                imobilFilter.MpMax = int.Parse(mpMax);
            }

            if (page != null)
            {
                imobilFilter.Page = int.Parse(page);
            }

            if (mapMode != null)
            {
                imobilFilter.MapMode = mapMode;
            }

            if (!string.IsNullOrEmpty(cartier))
            {
                imobilFilter.CartierId = int.Parse(cartier);
            }

            if (!string.IsNullOrEmpty(vanzator))
            {
                imobilFilter.TipVanzator = vanzator.EnumParse<TipVanzator>();
            }

            var routeValueDictionary = UrlFilterConverter.GetRouteValueDictionary(imobilFilter, this.unitOfWork);

            string urlParametersString = ConstructUrlParametersString(routeValueDictionary);

            return urlParametersString;
        }

        /// <summary>
        /// Can be called through link directly OR by SearchForValue which dynamically constructs the link
        /// </summary>
        /// <param name="judetName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("Anunturi/Lista/{*filters}")]
        //https://localhost:7034/Anunturi/judet-Cluj - deprecated
        [Route("Anunturi/judet-{judetName}/{*filters}")]
        public ActionResult AnunturiList(string filters)
        {
            var imobilFilter = new ImobilFilter() { StareAprobare = StareAprobare.Aprobat };

            //Put in try/catch because Single will throw error for older URL's
            try
            {
                if (filters != null)
                {
                    UrlFilterConverter.GenerateImobilFilter(this.unitOfWork.JudetRepository, this.unitOfWork.OrasRepository, this.unitOfWork.CartierRepository, imobilFilter, filters);
                }
            }
            catch (InvalidOperationException ex)
            {
                //Special case when Localitate name has changed and google crawler accesses old links
                log.ErrorFormat("Attempt to access invalid url: {0}", ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var imobilsData = GetImobilsData(
                imobilFilter.OrasId,
                imobilFilter.CartierId,
                imobilFilter.TipVanzator,
                TipOfertaDisplayFormatter.GetDisplayStringForUrl(imobilFilter.TipProprietate, imobilFilter.TipOfertaGen),
                imobilFilter.Page,
                imobilFilter.PretMin,
                imobilFilter.PretMax,
                imobilFilter.MpMin,
                imobilFilter.MpMax,
                imobilFilter.CamereMin,
                imobilFilter.CamereMax,
                imobilFilter.VechimeMaximaZile,
                imobilFilter.JudetId,
                imobilFilter.MapMode);

            //Add Agentii Imobiliare din Oras
            if (imobilFilter.OrasId != 0)
            {
                //TODO: Get only those with poze
                var agentii = this.unitOfWork.AgentiiRepository.GetAgentiiForOras(imobilFilter.OrasId, onlyWithPoza: true);
                if (agentii.Count > 0)
                {
                    ViewBag.AgentiiImobiliare = agentii;
                }
            }

            return this.View(imobilsData);
        }

        public ActionResult GetFilteredResultsOld(string judetName, string filters)
        {
            filters = "judet-" + judetName + "/" + filters;
            return RedirectToActionPermanent(nameof(AnunturiList), new { filters });
        }

        public ActionResult GetFilteredResultsOld2(string judetName, string filters)
        {
            filters = "judet-" + judetName + "/" + filters;
            return RedirectToActionPermanent(nameof(AnunturiList), new { filters });
        }

        public ActionResult SugestiiCautare(string text)
        {
            var locations = this.unitOfWork.OrasRepository.GetMatchingLocations(text, true);
            var sugestiiCautareData = new SugestiiCautareData(text, locations.Values.ToList());
            return this.View(sugestiiCautareData);
        }

        //Until here Anunturi List
        //---------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Route("Localitate-{orasName}/{tipOferta}/{titlu}/{imobilId}")]
        public ActionResult ApartamentDetalii(string imobilId, string titlu)
        {
            if (imobilId == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            int parsedImobilId;
            bool isNumeric = int.TryParse(imobilId, out parsedImobilId);

            Imobil imobil;
            if (isNumeric)
            {
                imobil = this.unitOfWork.AnunturiRepository.SingleOrDefault(x => x.Id == parsedImobilId);
                if (imobil == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            //Check if title changed in the meantime, do 301 redirect in case
            var url = UrlBuilder.BuildAnuntPageUrl(imobil.Oras.Nume, imobil.TipProprietate, imobil.TipOfertaGen, imobil.Title, imobil.Id);

            var newTitle = url.Split('/')[3];

            if (!string.IsNullOrEmpty(titlu) && titlu != newTitle)
            {
                log.WarnFormat("Redirect permanent because of anunt title change for anunt {0} with original title {1} and updated title {2}", imobil.Id, titlu, newTitle);
                //TODO reenable redirect
                //return RedirectPermanent(url);
            }

            //User of anunt - different of null all the time
            var userProfile = this.unitOfWork.UsersRepository.GetUserProfileById(imobil.UserId, false);
            var apartamentDetaliiData = new ApartamentDetaliiData { Imobil = imobil, UserProfile = userProfile };
            apartamentDetaliiData.AnunturiSimilare = this.unitOfWork.AnunturiRepository.GetSimilarAnunturi(parsedImobilId);

            var name = string.Empty;
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                name = User.Identity.Name;

                //Check if logged in user is admin
                var user = this.unitOfWork.UsersRepository.Single(x => x.Email == name);
                if (user != null)
                {
                    if (user.Role == Role.Administrator)
                    {
                        apartamentDetaliiData.IsCurrentUserAdmin = true;
                    }

                    apartamentDetaliiData.LoggedInUserId = user.Id;
                }
            }
            //TODO: REENABLE
            //if (!Request.Iscrawler() && IpHistoryCaching.ShouldLogFrequentAccesor(httpContext.Connection.RemoteIpAddress.ToString()))
            //{
            //    log.DebugFormat("User {0} accessed anunt with id {1}, ip: {2}", name != string.Empty ? name : "GUEST", imobilId.ParseToInt(), httpContext.Connection.RemoteIpAddress.ToString());
            //    //if(!apartamentDetaliiData.IsCurrentUserAdmin)
            //    this.unitOfWork.AnunturiRepository.IncrementNumarAccesari(parsedImobilId);
            //    this.unitOfWork.Complete();
            //}

            return View(apartamentDetaliiData);
        }

        public ActionResult AnunturiUtilizator(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            var userProfile = this.unitOfWork.UsersRepository.GetUserProfileById(userId, false);

            if (userProfile == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            int totalNumberOfImobils;
            var imobilsData = new ImobilsData(this.unitOfWork.AnunturiRepository.GetAllImobil(new ImobilFilter() { UserProfile = userProfile, PageSize = 50 }, out totalNumberOfImobils), null, 50);
            imobilsData.UserProfile = userProfile;
            return View(imobilsData);
        }

        [HttpPost]
        public ActionResult GetAnunturiForLocalitate(ImobilFilter imobilFilter, string latLong)
        {
            int totalNumberOfPages;
            imobilFilter.OnlyWithGoogleMarker = true;
            imobilFilter.GoogleMapBounds = latLong.Replace("%2C%20", ",");
            imobilFilter.StareAprobare = StareAprobare.Aprobat;
            imobilFilter.Page = 1;
            imobilFilter.PageSize = 100;

            var anunturiForLocation = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter, out totalNumberOfPages);

            string anunturiResult = string.Empty;
            //TODO delete randomize :D
            foreach (var imobil in anunturiForLocation/*.Randomize().Take(3).ToList()*/)
            {
                string poza = imobil.FirstPhoto;

                anunturiResult += imobil.Id + ";" + imobil.Suprafata + ";" + imobil.Price + ";" + poza + ";"
                                  + imobil.GoogleMarkerCoordinates + ";"
                                  + HtmlHelpers.RemoveSpecialCharacters(imobil.Title, '-', false) + ";"
                                  + HtmlHelpers.RemoveSpecialCharacters(imobil.Oras.Nume, '-', false) + ";"
                                  + TipOfertaDisplayFormatter.GetDisplayStringForUrl(imobil.TipProprietate, imobil.TipOfertaGen) + "|";
            }

            return this.Json(anunturiResult);
        }

        private string ConstructUrlParametersString(RouteValueDictionary routeValueDictionary)
        {
            var parameterString = string.Empty;
            foreach (var value in routeValueDictionary)
            {
                parameterString += value.Key + "-" + value.Value + "/";
            }
            return parameterString.TrimEnd('/');
        }

        private ImobilsData GetImobilsData(
            int? orasId,
            int? cartierId,
            TipVanzator? tipVanzator,
            string tipOferta,
            int? page,
            int? pretMin,
            int? pretMax,
            int? supMin,
            int? supMax,
            int? camereMin,
            int? camereMax,
            int? vechimeMaximaZile,
            int? judetId,
            string mapMode)
        {
            var pageSize = this.unitOfWork.SystemSettingsRepository.SystemSettings.DefaultPageSizeMainPages;
            var imobilFilter = new ImobilFilter
            {
                JudetId = judetId.GetValueOrDefault(),
                OrasId = orasId.GetValueOrDefault(),
                CartierId = cartierId.GetValueOrDefault(),
                TipVanzator = tipVanzator.GetValueOrDefault(),
                TipProprietate = TipOfertaDisplayFormatter.GetTipProprietateFromString(tipOferta),
                TipOfertaGen = TipOfertaDisplayFormatter.GetTipOfertagenFromString(tipOferta),
                Page = page.GetValueOrDefault(),
                PretMin = pretMin.GetValueOrDefault(),
                PretMax = pretMax.GetValueOrDefault(),
                MpMin = supMin.GetValueOrDefault(),
                MpMax = supMax.GetValueOrDefault(),
                CamereMin = camereMin.GetValueOrDefault(),
                CamereMax = camereMax.GetValueOrDefault(),
                VechimeMaximaZile = vechimeMaximaZile.GetValueOrDefault(),
                StareAprobare = StareAprobare.Aprobat,
                PageSize = pageSize,
                MapMode = mapMode,
                //We need also latlong here
                //OnlyWithGoogleMarker = mapMode == "true"
            };
            int totalNumberOfImobils;
            var allImobils = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter, out totalNumberOfImobils);

            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfImobils / pageSize);

            var imobilsData = new ImobilsData(allImobils, imobilFilter, displayPageNumber);

            imobilsData.PromovatedImobils =
                this.unitOfWork.AnunturiRepository.GetPromovatedImobils(imobilFilter, PromovatLevel.PromovatNormal);

            imobilsData.LastAddedImobils = this.unitOfWork.AnunturiRepository.GetLastAddedImobils(4);

            imobilsData.TotalNumberOfEntries = totalNumberOfImobils;
            imobilsData.Judets = this.unitOfWork.JudetRepository.Judete();
            if (imobilFilter.JudetId != 0)
            {
                imobilsData.Orases = this.unitOfWork.OrasRepository.GetSelectableOrases(imobilFilter.JudetId);
            }

            imobilsData.Cartiers = this.unitOfWork.CartierRepository.GetSelectableCartiers(imobilFilter.OrasId);
            imobilsData.SelectableOrasesWithNumber =
                this.unitOfWork.OrasRepository.GetSelectableOrasesWithNumber(imobilFilter);
            if (imobilFilter.JudetId != 0)
            {
                imobilsData.TotalSelectableOrasesWithNumber = totalNumberOfImobils == 0
                    ? this.unitOfWork.OrasRepository.GetSelectableOrasesWithNumber(new ImobilFilter()
                    { JudetId = imobilFilter.JudetId })
                    : new Dictionary<Oras, int>();
            }

            imobilsData.SelectableCartiersWithNumber =
                this.unitOfWork.CartierRepository.GetSelectableCartiersWithNumber(imobilFilter);

            //In caz ca ramane selectat un cartier si orasul curent nu are cartiere
            if (imobilsData.Cartiers.Count == 0)
            {
                imobilFilter.CartierId = 0;
            }

            if (imobilFilter.OrasId != 0
                && imobilsData.Orases.First(x => x.Id == imobilFilter.OrasId).Nume != "Alta Localitate")
            {
                imobilsData.GpsCoordinates =
                    "(" + this.unitOfWork.OrasRepository.GetOrasGpsCoordinates(imobilFilter.OrasId) + ")";
            }
            else if (imobilFilter.JudetId != 0)
            {
                imobilsData.GpsCoordinates =
                    "(" + this.unitOfWork.JudetRepository.GetJudetGpsCoordinates(imobilFilter.JudetId) + ")";
                imobilsData.ZoomLevel = 10;
            }
            else
            {
                //log.ErrorFormat("No judetId or orasId defined for getAnunturiList...");
                imobilsData.GpsCoordinates = "(46.777248,23.599889)";
                imobilsData.ZoomLevel = 8;
            }

            if (!string.IsNullOrEmpty(imobilFilter.MapMode))
            {
                imobilsData.MapMode = imobilFilter.MapMode;
            }

            return imobilsData;
        }
    }
}
