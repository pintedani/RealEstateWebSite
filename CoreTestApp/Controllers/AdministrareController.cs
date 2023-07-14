using CoreTestApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imobiliare.UI.Controllers
{
    [Authorize]
    public class AdministrareController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 10;

        private readonly IUnitOfWork unitOfWork;

        private readonly IEmailManagerService emailManagerService;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AdministrareController));

        public AdministrareController(
          IUnitOfWork unitOfWork,
          IEmailManagerService emailManagerService)
        {
            this.emailManagerService = emailManagerService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// gets all the anunturi for the curent user or the userid
        /// </summary>
        /// <param name="userId"> userId for which to get anunturi </param>
        public ActionResult Index(string userId, int? JudetSelect, int? OrasSelect, int? CartierSelect, StareAprobare? StareAprobareSelect, int? page)
        {
            int judetId = 0;

            if (JudetSelect != null)
            {
                judetId = JudetSelect.Value;
            }

            int orasId = 0;
            if (OrasSelect != null)
            {
                orasId = OrasSelect.Value;
            }

            int cartierId = 0;
            if (CartierSelect != null)
            {
                cartierId = CartierSelect.Value;
            }

            int selectedPageFinal = 1;
            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            var stareAprobare = StareAprobare.Toate;
            if (StareAprobareSelect != null)
            {
                stareAprobare = StareAprobareSelect.Value;
            }

            List<Imobil> apartmentsForCurrentUser = null;
            var imobilFilter = new ImobilFilter();

            UserProfile userProfile;
            //First two for If admin wants to view anunturi
            if (userId != null)
            {
                userProfile = this.unitOfWork.UsersRepository.GetUserProfileById(userId, false);
            }
            else if (Request.Form["userIdSelect"] != null)
            {
                userProfile = this.unitOfWork.UsersRepository.GetUserProfileById(Request.Form["userIdSelect"], false);
            }
            else
            {
                //Default value
                userProfile = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            }

            int totalNumberOfImobils = 0;
            if (User.Identity.IsAuthenticated)
            {
                imobilFilter = new ImobilFilter
                {
                    UserProfile = userProfile,
                    Page = selectedPageFinal,
                    PageSize = DEFAULT_PAGE_SIZE
                };
                imobilFilter.JudetId = judetId;
                imobilFilter.OrasId = orasId;
                imobilFilter.CartierId = cartierId;
                imobilFilter.StareAprobare = stareAprobare;
                apartmentsForCurrentUser = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter, out totalNumberOfImobils);
            }

            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfImobils / DEFAULT_PAGE_SIZE);

            var imobilsData = new ImobilsData(apartmentsForCurrentUser, imobilFilter, displayPageNumber);
            imobilsData.TotalNumberOfEntries = totalNumberOfImobils;
            imobilsData.Judets = this.unitOfWork.JudetRepository.Judete();
            imobilsData.Orases = this.unitOfWork.OrasRepository.GetSelectableOrases(imobilFilter.JudetId);
            imobilsData.Cartiers = this.unitOfWork.CartierRepository.GetSelectableCartiers(imobilFilter.OrasId);
            imobilsData.UserProfile = userProfile;

            return View(imobilsData);
        }

        [HttpGet]
        public ActionResult AnuntEditare(int? id, string tipOferta)
        {
            var userProfile = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            if (userProfile.Role != Role.Administrator)
            {
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, "User clicked adauga anunt", userName: User.Identity.Name);
                this.unitOfWork.Complete();
            }
            var tipOft = TipProprietate.Apartament;
            if (tipOferta != null)
            {
                tipOft = tipOferta.EnumParse<TipProprietate>();
            }

            var imobilViewModel = new ImobilViewModel();
            imobilViewModel.TipProprietate = tipOft;
            if (userProfile.Role != Role.Administrator)
            {
                imobilViewModel.TipVanzator = userProfile.TipVanzator;
                imobilViewModel.ContactTelefon = userProfile.PhoneNumber;
            }

            if (id != null && id != 0)
            {
                this.CheckRights(id.Value);

                var imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == id);
                imobilViewModel.Id = imobil.Id;
                imobilViewModel.Title = imobil.Title;
                imobilViewModel.TipOfertaGen = imobil.TipOfertaGen;
                imobilViewModel.TipProprietate = imobil.TipProprietate;
                imobilViewModel.TipVanzator = imobil.TipVanzator;
                imobilViewModel.Descriere = imobil.Descriere;
                imobilViewModel.Poze = imobil.Poze;
                imobilViewModel.Price = imobil.Price;
                imobilViewModel.Suprafata = imobil.Suprafata;
                imobilViewModel.ContactTelefon = imobil.ContactTelefon;
                imobilViewModel.ContactEmail = imobil.ContactEmail;
                imobilViewModel.Oras = imobil.Oras;
                imobilViewModel.OrasId = imobil.Oras.Id;
                imobilViewModel.Judet = imobil.Judet;
                imobilViewModel.GoogleMarkerCoordinates = imobil.GoogleMarkerCoordinates;
                imobilViewModel.Strada = imobil.Strada;
                imobilViewModel.VechimeImobil = imobil.VechimeImobil;
                imobilViewModel.Negociabil = imobil.Negociabil;
                imobilViewModel.AerConditionat = imobil.AerConditionat;
                imobilViewModel.CT = imobil.CT;
                imobilViewModel.Garaj = imobil.Garaj;
                imobilViewModel.LocParcare = imobil.LocParcare;
                imobilViewModel.Finisat = imobil.Finisat;
                imobilViewModel.Decomandat = imobil.Decomandat;
                imobilViewModel.LocInPivnita = imobil.LocInPivnita;
                imobilViewModel.Valabilitate = imobil.Valabilitate;
                imobilViewModel.Etaj = imobil.Etaj;
                imobilViewModel.NumarTotalEtaje = imobil.NumarTotalEtaje;
                imobilViewModel.NumarCamere = imobil.NumarCamere;
                imobilViewModel.NrBai = imobil.NrBai;
                imobilViewModel.NrBalcoane = imobil.NrBalcoane;
                imobilViewModel.ObservatiiAdmin = imobil.ObservatiiAdmin;
                imobilViewModel.StareAprobare = imobil.StareAprobare;
                imobilViewModel.PromotedLevel = imobil.PromotedLevel;

                if (imobil.Cartier != null)
                {
                    imobilViewModel.CartierId = imobil.Cartier.Id;
                }

                //Can this be null?
                if (imobil.Oras != null)
                {
                    imobilViewModel.GpsCoordinates = "(" + this.unitOfWork.OrasRepository.GetOrasGpsCoordinates(imobil.Oras.Id) + ")";
                    var cartiere = this.unitOfWork.CartierRepository.GetSelectableCartiers(imobil.Oras.Id);
                    if (cartiere.Any())
                    {
                        cartiere.Insert(0, new Cartier { Id = 0, Nume = "Nespecificat" });
                    }

                    imobilViewModel.Cartiere = cartiere;
                }
                else if (imobilViewModel.Judet != null)
                {
                    imobilViewModel.GpsCoordinates = "(" + this.unitOfWork.JudetRepository.GetJudetGpsCoordinates(imobilViewModel.Judet.Id) + ")";
                    imobilViewModel.ZoomLevel = 10;
                }
                else
                {
                    //Should never occur - Avrig :)
                    imobilViewModel.GpsCoordinates = "(46.777248,23.599889)";
                    imobilViewModel.ZoomLevel = 8;
                }
            }

            imobilViewModel.Judete = this.unitOfWork.JudetRepository.Judete();
            imobilViewModel.UserProfile = userProfile;

            return this.View(imobilViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnuntEditare([Bind(Include = "Id,Title,Descriere,Price,Suprafata,ContactTelefon,ContactEmail,OrasId,CartierId,TipOfertaGen,TipProprietate,TipVanzator,GoogleMarkerCoordinates,NrBalcoane,NumarCamere,NrBai,NrBalcoane,Strada,VechimeImobil,Negociabil,Decomandat,AerConditionat,CT,Garaj,LocParcare,Finisat,LocInPivnita,Valabilitate,Etaj,NumarTotalEtaje,PromotedLevel")]
            ImobilViewModel imobilViewModel)
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            if (ModelState.IsValid)
            {
                //Adaugare logic
                if (imobilViewModel.IsModAdaugare)
                {
                    var imobilDto = InitializeToBeEditedAnunt(imobilViewModel);

                    var addedImobil = this.unitOfWork.AnunturiRepository.AddImobil(imobilDto, HttpContext.User.Identity.Name);
                    this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Added anunt by user {User.Identity.Name}, de pe dispozitiv mobil {Request.Browser.IsMobileDevice}, adresa ip: {Request.UserHostAddress}, browser: {Request.UserAgent}", userName: User.Identity.Name);
                    this.unitOfWork.Complete();
                    if (user.Role == Role.Administrator || user.TrustedUser)
                    {
                        TempData["Message"] = "Anunt adaugat cu succes, acesta este vizibil pe site. Va multumim!";
                    }
                    else
                    {
                        TempData["Message"] = "Anunt adaugat cu succes, acesta va fi analizat de catre unul dintre moderatori, dupa care va fi aprobat pe site. Va multumim!";
                    }

                    return this.RedirectToAction(nameof(AnuntEditare), new { id = addedImobil.Id });
                }
                //Edit logic
                else
                {
                    this.CheckRights(imobilViewModel.Id);

                    try
                    {
                        Imobil imobilDto = InitializeToBeEditedAnunt(imobilViewModel);

                        this.unitOfWork.AnunturiRepository.UpdateImobilNew(imobilDto, user.Role == Role.Administrator);
                        this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(imobilViewModel.Id, "Editat de " + User.Identity.Name + "(" + DateTime.Now.ToShortDateString() + ")");
                        this.unitOfWork.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        TempData["ErrorMessage"] = "Eroare la updatarea anuntului. S-a modificat intre timp din alta sesiune! Incercati din nou.";
                        log.ErrorFormat("A aparut o eroare la updatarea anuntului {0} deoarece s-a modificat intre timp! by user {1}", imobilViewModel.Id, User.Identity.Name);
                    }
                    if (this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name).Role == Role.NormalUser)
                    {
                        this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Updated imobil with id {imobilViewModel.Id}", userName: User.Identity.Name, notifyAdmin: true);
                        this.unitOfWork.Complete();
                    }

                    TempData["Message"] = "Anunt editat cu succes!";
                    return this.RedirectToAction("Index", "Administrare");
                }
            }

            imobilViewModel.UserProfile = user;
            imobilViewModel.Judete = this.unitOfWork.JudetRepository.Judete();
            return this.View(imobilViewModel);
        }

        private Imobil InitializeToBeEditedAnunt(ImobilViewModel imobilViewModel)
        {
            var imobilDto = new Imobil();
            imobilDto.Id = imobilViewModel.Id;
            imobilDto.Title = imobilViewModel.Title;
            imobilDto.Descriere = imobilViewModel.Descriere;
            imobilDto.TipOfertaGen = imobilViewModel.TipOfertaGen;
            imobilDto.TipProprietate = imobilViewModel.TipProprietate;
            imobilDto.TipVanzator = imobilViewModel.TipVanzator;
            imobilDto.Price = imobilViewModel.Price;
            imobilDto.Suprafata = imobilViewModel.Suprafata;
            imobilDto.ContactTelefon = imobilViewModel.ContactTelefon;
            imobilDto.ContactEmail = imobilViewModel.ContactEmail;
            imobilDto.GoogleMarkerCoordinates = imobilViewModel.GoogleMarkerCoordinates;
            imobilDto.Oras = this.unitOfWork.OrasRepository.Single(x => x.Id == imobilViewModel.OrasId);
            if (imobilViewModel.CartierId != 0)
            {
                imobilDto.Cartier = this.unitOfWork.CartierRepository.Single(x => x.Id == imobilViewModel.CartierId);
            }

            imobilDto.Strada = imobilViewModel.Strada;
            imobilDto.VechimeImobil = imobilViewModel.VechimeImobil;
            imobilDto.Negociabil = imobilViewModel.Negociabil;
            imobilDto.AerConditionat = imobilViewModel.AerConditionat;
            imobilDto.CT = imobilViewModel.CT;
            imobilDto.Garaj = imobilViewModel.Garaj;
            imobilDto.LocParcare = imobilViewModel.LocParcare;
            imobilDto.Finisat = imobilViewModel.Finisat;
            imobilDto.Decomandat = imobilViewModel.Decomandat;
            imobilDto.LocInPivnita = imobilViewModel.LocInPivnita;
            imobilDto.Valabilitate = imobilViewModel.Valabilitate;
            imobilDto.Etaj = imobilViewModel.Etaj;
            imobilDto.NumarTotalEtaje = imobilViewModel.NumarTotalEtaje;
            imobilDto.NumarCamere = imobilViewModel.NumarCamere;
            imobilDto.NrBai = imobilViewModel.NrBai;
            imobilDto.NrBalcoane = imobilViewModel.NrBalcoane;
            imobilDto.PromotedLevel = imobilViewModel.PromotedLevel;
            return imobilDto;
        }

        public ActionResult StergeGoogleMarker(int imobilId)
        {
            this.unitOfWork.AnunturiRepository.RemoveGoogleMarkerCoordinates(imobilId);
            this.unitOfWork.Complete();
            log.DebugFormat("Removed google marker for Imobil with id {0} by user {1}", imobilId, User.Identity.Name);
            return Json("Marker sters cu success");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteImage()
        {
            var imobilId = Request.Form["imobilId"].ParseToInt();
            var photoToDelete = Request.Form["pozaDeSters"];
            var movePozaUp = Request.Form["movePozaUp"];
            var movePozaDown = Request.Form["movePozaDown"];
            var rotatePoza = Request.Form["rotatePoza"];
            if (photoToDelete != null)
            {
                log.DebugFormat("Photo {0} deleted by {1}", photoToDelete, User.Identity.Name);
                this.unitOfWork.AnunturiRepository.DeleteImage(imobilId, photoToDelete);
                this.unitOfWork.Complete();
            }
            if (movePozaUp != null)
            {
                log.DebugFormat("Photo {0} moved up by {1}", movePozaUp, User.Identity.Name);
                this.unitOfWork.AnunturiRepository.MovePhotoUp(imobilId, movePozaUp);
                this.unitOfWork.Complete();
            }
            if (movePozaDown != null)
            {
                log.DebugFormat("Photo {0} moved down by {1}", movePozaDown, User.Identity.Name);
                this.unitOfWork.AnunturiRepository.MovePhotoDown(imobilId, movePozaDown);
                this.unitOfWork.Complete();
            }
            if (rotatePoza != null)
            {
                log.DebugFormat("Rotate photo {0} by {1}", rotatePoza, User.Identity.Name);
                this.unitOfWork.AnunturiRepository.RotatePhoto(imobilId, rotatePoza);
                this.unitOfWork.Complete();
            }
            var imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == imobilId);
            return PartialView("_pozeEditAnuntPartial", new ImobilViewModel { Poze = imobil.Poze });
        }

        /// <summary>
        /// AllowAnonymous in order to work with uploadify which does not work with authorized stuff
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddImage(string currentValue)
        {
            HttpPostedFileBase file = Request.Files["Filedata"];

            int imobilId = currentValue.ParseToInt();
            var imageName = this.unitOfWork.AnunturiRepository.AddImages(imobilId, new[] { file });
            this.unitOfWork.Complete();
            log.DebugFormat("Added images for imobil async with id {0} by user {1}, imageName: {2}", imobilId, User.Identity.Name, imageName);
            var imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == imobilId);
            return PartialView("_pozeEditAnuntPartial", new ImobilCreateData { ImobilToEdit = imobil });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddImageNoAjax(HttpPostedFileBase file)
        {
            int imobilId = int.Parse(Request.Form["imobilId"]);
            if (file != null)
            {
                var imageName = this.unitOfWork.AnunturiRepository.AddImages(imobilId, new[] { file });
                if (!string.IsNullOrEmpty(imageName))
                {
                    this.unitOfWork.Complete();
                    log.DebugFormat("Added images for imobil NOT async with id {0} by user {1}, imageName: {2}",
                        imobilId, User.Identity.Name, imageName);
                }
                else
                {
                    TempData["ErrorMessage"] = "Eroare la incarcarea fisierului. Verificati sa fie de tipul imagine!";
                }
            }
            else
            {
                log.DebugFormat("No image selected for imobil NOT async  by user {0}", User.Identity.Name);
            }
            return RedirectToAction(nameof(AnuntEditare), new RouteValueDictionary(new Dictionary<string, object>() { { "id", imobilId } }));
        }

        public ActionResult Deactivare(int imobilid, string returnUrl)
        {
            this.CheckRights(imobilid);
            this.unitOfWork.AnunturiRepository.DeactivareAnunt(imobilid);
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"User deactivated imobil with id {imobilid}", userName: User.Identity.Name, notifyAdmin: true);
            this.unitOfWork.Complete();
            return Redirect(returnUrl);
        }

        public ActionResult ReActivare(int imobilid, string returnUrl)
        {
            this.CheckRights(imobilid);
            this.unitOfWork.AnunturiRepository.ReactivareAnunt(imobilid);
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"User reactivated imobil with id {imobilid}", userName: User.Identity.Name, notifyAdmin: true);
            this.unitOfWork.Complete();
            //Tempdata survives redirect si se poate testa pe view
            //Tutorial bootstrap John Galloway
            TempData["Message"] = "Anunt reactivat cu succes";
            return Redirect(returnUrl);
        }

        public ActionResult ApartamentReactualizare(int imobilid, string returnUrl)
        {
            this.CheckRights(imobilid);
            this.unitOfWork.AnunturiRepository.ReActualizareAnunt(imobilid);
            this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(imobilid, "User react(" + DateTime.Now.ToShortDateString() + ")");
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"User renewed imobil with id {imobilid}", userName: User.Identity.Name, notifyAdmin: true);
            this.unitOfWork.Complete();
            TempData["Message"] = "Anunt reactualizat cu succes";
            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        //Schimba stare anunt prin email link mainly
        public ActionResult SchimbaStareAnunt(string secretNumber, int anuntId, StareAprobare stare)
        {
            log.DebugFormat("User requested from EMAIL, change of imobil state with id {0}, userId {1} to state {2} ", secretNumber, anuntId, stare);

            var anunt = this.unitOfWork.AnunturiRepository.Single(x => x.Id == anuntId);

            //Case 1: Invalid link
            if (anunt.UserId != secretNumber)
            {
                log.ErrorFormat("User requestes change of imobil state {0} from EMAIL with invalid user! {1}", anuntId, secretNumber);
                ViewBag.Message = string.Format("User requestes change of imobil state {0} from EMAIL with invalid user! {1}", anuntId, secretNumber);
                return RedirectToAction("Index");
            }

            //Case 1: Only expired or approved can be renewed
            //Se poate actualiza doar daca este expirat sau Aprobat
            if (anunt.StareAprobare != StareAprobare.Expirat && anunt.StareAprobare != StareAprobare.Aprobat)
            {
                log.ErrorFormat("Eroare la actualizare anuntului cu id {0} din starea {1} in {2} prin email. Operatiune restrictionata.", anuntId, anunt.StareAprobare, stare);
                TempData["Message"] = String.Format("Eroare la actualizare anuntului cu id {0} din starea {1} in {2} prin email. Operatiune restrictionata.", anuntId, anunt.StareAprobare, stare);
                return RedirectToAction("Index");
            }

            //Case 2: Se poate actualiza doar o data pe zi, dezactivarea nu e afectata

            //Put this anunt.StareAprobare != StareAprobare.Expirat on monitoring, because of some condition in logs it was DataAdaugare=DataAprobare and anunt = Expirat(Probabil at Reactivare DateTime now was not set)
            if (anunt.StareAprobare != StareAprobare.Expirat)
            {
                if (stare == StareAprobare.Aprobat && anunt.DataAdaugare == anunt.DataAprobare)
                {
                    log.ErrorFormat(
                      "Nu se poate actualiza anuntul cu id {0} din starea {1} in {2} prin email. Reactualizarea se poate efectua doar o data pe zi.",
                      anuntId, anunt.StareAprobare, stare);
                    TempData["Message"] = "Anuntul este deja actualizat! Reactualizarea se poate efectua doar o data pe zi.";
                    return RedirectToAction("Index");
                }
            }

            this.unitOfWork.AnunturiRepository.ChangeImobilState(anuntId, stare);
            this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(anuntId, "Schimbat stare prin email la " + stare + " (" + DateTime.Now.ToShortDateString() + ")");

            this.unitOfWork.AuditTrailRepository.AddAuditTrail(
              AuditTrailCategory.Message,
              "Schimbat stare prin email la " + stare + " (" + DateTime.Now.ToShortDateString() + "). Anunt id " + anunt.Id
              + ", Titlu: " + anunt.Title,
              notifyAdmin: true);
            this.unitOfWork.Complete();

            return RedirectToAction("StareUpdatareEmailAnunt", new
            {
                anuntID = anuntId,
                stareAprobare = stare
            });

        }

        [AllowAnonymous]
        public ActionResult StareUpdatareEmailAnunt(int anuntID, StareAprobare stareAprobare)
        {
            ViewBag.Message = String.Format("Anuntul cu id {0} actualizat cu succes. Stare actuala: {1}. Intrati in cont pentru a vizualiza anunturile dumneavoastra.", anuntID, stareAprobare);
            ViewBag.AnuntId = anuntID;
            ViewBag.StareAprobare = stareAprobare;
            return View();
        }

        [AllowAnonymous]
        public ActionResult DezabonareNewsLetter(string userid)
        {
            this.unitOfWork.UsersRepository.DezaboneazaNewsletter(userid);
            this.unitOfWork.Complete();
            log.WarnFormat("Userul cu id {0} s-a dezabonat de la newsletter prin email.", userid);
            TempData["Message"] = "Ati fost dezabonat de la newsletter.";
            return RedirectToAction("Index");
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
