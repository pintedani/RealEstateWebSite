using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.Repositories;
using Imobiliare.ServiceLayer.Interfaces;
using Imobiliare.UI.Models.SuperAdmin;
using Imobiliare.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Imobiliare.Managers.ExtensionMethods;
using Logging;
using System.Drawing.Printing;

namespace Imobiliare.UI.Controllers
{
    /// <summary>
    /// Used for site administrators to approve requests, change user details, etc
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class SuperAdminController : Controller
    {
        private DateTimeFormatInfo dateFormat = new DateTimeFormatInfo { ShortDatePattern = "dd'/'MM'/'yyyy" };

        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IEmailManagerService emailManagerService;
        private readonly IExternalSiteParserService externalSiteContentParser;

        private static readonly ILog log = LogManager.GetLogger(typeof(SuperAdminController));

        public SuperAdminController(
          IUnitOfWork unitOfWork,
          IEmailManagerService emailManagerService,
          IExternalSiteParserService externalSiteContentParser,
          IWebHostEnvironment hostingEnvironment)
        {
            this.emailManagerService = emailManagerService;
            this.externalSiteContentParser = externalSiteContentParser;
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult Salam()
        {
            return Ok();
        }

        public ActionResult Index(
          int? JudetSelect,
          int? OrasSelect,
          int? CartierSelect,
          TipVanzator? TipVanzatorSelect,
          StareAprobare? StareAprobareSelect,
          int? page,
          bool? OnlyUserAnunturi,
          bool? OnlyAdminAnunturi,
          string LogStartDate,
          string LogEndDate,
          string UserName,
          bool? FiltareDupaDataAdaugareInitiala)
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

            var tipVanzator = TipVanzator.TotiVanzatorii;
            if (TipVanzatorSelect != null)
            {
                tipVanzator = TipVanzatorSelect.Value;
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

            bool onlyUserAnunturi = false;
            if (OnlyUserAnunturi != null)
            {
                onlyUserAnunturi = OnlyUserAnunturi.Value;
            }

            bool onlyAdminAnunturi = false;
            if (OnlyAdminAnunturi != null)
            {
                onlyAdminAnunturi = OnlyAdminAnunturi.Value;
            }

            DateTime logStartDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(LogStartDate))
            {
                logStartDate = DateTime.ParseExact(LogStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            DateTime logEndDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(LogEndDate))
            {
                logEndDate = DateTime.ParseExact(LogEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            UserProfile userprofile = null;
            if (!string.IsNullOrEmpty(UserName))
            {
                userprofile = this.unitOfWork.UsersRepository.GetUserProfiles().FirstOrDefault(x => x.UserName.Contains(UserName));
            }

            bool filtareDupaDataAdaugareInitiala = false;
            if (FiltareDupaDataAdaugareInitiala != null)
            {
                filtareDupaDataAdaugareInitiala = FiltareDupaDataAdaugareInitiala.Value;
            }
            var pageSize = this.unitOfWork.SystemSettingsRepository.SystemSettings.DefaultPageSizeAdminPageAnunturi;
            var imobilFilter = new ImobilFilter { PageSize = pageSize, Page = selectedPageFinal };
            imobilFilter.JudetId = judetId;
            imobilFilter.OrasId = orasId;
            imobilFilter.CartierId = cartierId;
            imobilFilter.TipVanzator = tipVanzator;
            imobilFilter.StareAprobare = stareAprobare;
            imobilFilter.OnlyUserAnunturi = onlyUserAnunturi;
            imobilFilter.OnlyAdminAnunturi = onlyAdminAnunturi;
            imobilFilter.PerioadaStart = logStartDate;
            imobilFilter.PerioadaEnd = logEndDate;
            imobilFilter.UserProfile = userprofile;
            imobilFilter.FiltareDupaDataAdaugareInitiala = filtareDupaDataAdaugareInitiala;

            int totalNumberOfImobils;
            var allImobils = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter, out totalNumberOfImobils);

            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfImobils / pageSize);
            var superAdminData = new SuperAdminData(allImobils, imobilFilter, displayPageNumber);
            superAdminData.TotalNumberOfEntries = totalNumberOfImobils;
            superAdminData.Judets = this.unitOfWork.JudetRepository.Judete();
            superAdminData.Orases = this.unitOfWork.OrasRepository.GetSelectableOrases(imobilFilter.JudetId);
            superAdminData.Cartiers = this.unitOfWork.CartierRepository.GetSelectableCartiers(imobilFilter.OrasId);

            superAdminData.EmailTemplates =
              this.emailManagerService.GetAllEmailTemplates()
                .Where(x => x.EmailTemplateCategory == EmailTemplateCategory.AdminAnuntRelated)
                .ToList();

            return View(superAdminData);
        }

        /// <summary>
        /// selectSingleUserEmail optional for also redirecting through actions in superadmin to specific user
        /// In order to be able to send email notifications from users view
        /// </summary>
        public ActionResult Users(string selectSingleUserEmail, string userName, string page, string tipVanzatorSelect, string tipRoleSelect)
        {
            var selectedPage = 1;
            if (!string.IsNullOrWhiteSpace(page))
            {
                selectedPage = int.Parse(page);
            }

            var tip = TipVanzator.TotiVanzatorii;
            if (tipVanzatorSelect != null)
            {
                tip = tipVanzatorSelect.EnumParse<TipVanzator>();
            }

            var role = Role.Toti;
            if (tipRoleSelect != null)
            {
                role = tipRoleSelect.EnumParse<Role>();
            }

            if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(selectSingleUserEmail))
            {
                userName = selectSingleUserEmail;
            }
            if (!string.IsNullOrEmpty(userName))
            {
                var userProfile = this.unitOfWork.UsersRepository.GetUserProfiles().FirstOrDefault(x => x.UserName.Contains(userName.Trim()));
                if (userProfile != null)
                {
                    return View(new UsersData()
                    {
                        UserProfiles = new List<UserProfile>() { userProfile },
                        EmailTemplates = this.emailManagerService.GetAllEmailTemplates()
                        .Where(x => x.EmailTemplateCategory == EmailTemplateCategory.AdminUserRelated)
                        .ToList()
                    });
                }
            }

            var pageSize = this.unitOfWork.SystemSettingsRepository.SystemSettings.DefaultPageSizeAdminPageUsers;
            var totalNumberOfUsers = 0;
            var userProfiles = this.unitOfWork.UsersRepository.GetUserProfiles(tip, role, selectedPage, pageSize, out totalNumberOfUsers);

            var displayPageNumber = (int)Math.Ceiling((double)totalNumberOfUsers / pageSize);

            var usersData = new UsersData
            {
                SelectedTipVanzator = tip,
                SelectedRole = role,
                UserProfiles = userProfiles,
                Page = selectedPage,
                NumberOfPages = displayPageNumber,
                EmailTemplates = this.emailManagerService.GetAllEmailTemplates()
                .Where(x => x.EmailTemplateCategory == EmailTemplateCategory.AdminUserRelated)
                .ToList()
            };

            //foreach(var user in userProfiles)
            //{
            //    if(user.Anunturi.Count == 0)
            //    {
            //        this.unitOfWork.UsersRepository.DeleteUser(user.Id);
            //        log.Debug($"SuperAdmin bulk hack delete empty users {User.Identity.Name} deleted user {user.Email}");
            //    }
            //}
            //this.unitOfWork.Complete();

            return View(usersData);
        }

        public ActionResult UserEditare(string userid)
        {
            UserProfile user = this.unitOfWork.UsersRepository.GetUserProfileById(userid, true);

            //using (var db = new ApplicationDbContext())
            //{
            //    user = db.Users.Single(u => u.Id == userid);
            //}
            return View(user);
        }

        public ActionResult UpdateUserInfo(UserProfile userProfile)
        {
            this.unitOfWork.UsersRepository.UpdateUserProfileByAdmin(userProfile);
            this.unitOfWork.Complete();

            log.Debug($"SuperAdmin Edited user {userProfile.UserName} by user {User.Identity.Name}");
            TempData["Message"] = $"User {userProfile.UserName} editat cu success";
            return RedirectToAction("Users", new { selectSingleUserEmail = userProfile.UserName });
        }

        public ActionResult UserStergere(string userid, string returnUrl)
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Id == userid);

            var anunturi = this.unitOfWork.AnunturiRepository.Find(x => x.UserId == user.Id);
            log.Warn($"Attempting to delete anunturi for user {user.UserName}, total anunturi: {anunturi.Count}");
            foreach (var anunt in anunturi)
            {
                this.unitOfWork.AnunturiRepository.DeleteImobil(anunt.Id, hostingEnvironment.WebRootPath);
            }

            this.unitOfWork.UsersRepository.DeleteUser(userid);
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"SuperAdmin {this.User.Identity.Name} deleted user {user.Email}", notifyAdmin: true);
            this.unitOfWork.Complete();
            log.Debug($"SuperAdmin {User.Identity.Name} deleted user {user.Email}");
            TempData["Message"] = $"SuperAdmin {User.Identity.Name} deleted user {user.Email}";
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Users");
            }
        }

        public ActionResult ChangeImobilState(string id, string stare, string returnUrl)
        {
            int convertedId = int.Parse(id);
            var stareAprobare = stare.EnumParse<StareAprobare>();

            var imobil = this.unitOfWork.AnunturiRepository.Single(x => x.Id == convertedId);
            var previousState = imobil.StareAprobare;
            imobil.StareAprobare = stareAprobare;
            if (stareAprobare == StareAprobare.Aprobat)
            {
                imobil.DataAprobare = DateTime.Now;
                imobil.DataAdaugare = DateTime.Now;

                //Send email confirmation ca s-a aprobat
                //Only to users which are not administrators
                //Only if previous state was "InAsteptare"
                var user = this.unitOfWork.UsersRepository.Single(x => x.Id == imobil.UserId);
                if (previousState == StareAprobare.InAsteptare && user.Role != Role.Administrator)
                {
                    this.emailManagerService.AnuntAprobatRelatedEmail(
                      imobil.Title,
                      convertedId,
                      user.Id,
                      user.UserName);
                }
                else if (user.Role != Role.Administrator)
                {
                    log.Debug($"Amin approved imobil {imobil.Title} with id {imobil.Id}, no email send needed because added by admin");
                }
                else if (previousState != StareAprobare.InAsteptare)
                {
                    log.Debug($"Amin approved imobil {imobil.Title} with id {imobil.Id}, no email send needed because was not InAsteptare before");
                }
            }
            this.unitOfWork.Complete();
            log.Debug($"Amin changed state of imobil {imobil.Title} with id {imobil.Id} to state {stare}");

            return this.Redirect(returnUrl.Replace("&amp;", "&"));
        }

        public ActionResult ConfirmaCont(string id)
        {
            var user = this.unitOfWork.UsersRepository.GetUserProfileById(id, false);
            return RedirectToAction("ConfirmEmail", "Account", new { userId = user.Id, code = user.ConfirmationToken });
        }

        //[ValidateInput(false)]
        public ActionResult SendEmailReactualizare(int emailConfirmareAnuntId, string emailConfirmareAnuntTitlu, string emailConfirmareAnuntEmail, string emailConfirmareUserId, string emailTemplateHumanReadableId, string returnUrl)
        {;
            //var mesaj = Request.Unvalidated["emailConfirmareAnuntMesaj"] ;
            //mesaj = mesaj.Replace("break", "<br/>");

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(emailConfirmareAnuntEmail).Success)
                throw new Exception("Invalid email parameter for sending Trimite Email user message");

            this.emailManagerService.AnuntRelatedEmail(
              emailTemplateHumanReadableId,
              emailConfirmareAnuntTitlu,
              emailConfirmareAnuntId,
              emailConfirmareUserId,
              emailConfirmareAnuntEmail,
              "");

            log.Debug($"Email send by admin for reactualizare to user with id {emailConfirmareAnuntId} and titlu {emailConfirmareAnuntTitlu}");
            this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(emailConfirmareAnuntId, "Trimis Email(" + emailTemplateHumanReadableId + " " + DateTime.Now.ToShortDateString() + ")");
            this.unitOfWork.Complete();
            return this.Redirect(returnUrl);
        }

        public ActionResult SendEmailToUser(string emailConfirmareAnuntEmail, string emailConfirmareUserId, string EmailTemplateHumanReadableId, string returnUrl)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(emailConfirmareAnuntEmail).Success)
                throw new Exception("Invalid email parameter for sending Trimite Email user message");

            this.emailManagerService.UserRelatedEmail(EmailTemplateHumanReadableId, emailConfirmareUserId, emailConfirmareAnuntEmail);

            log.Debug($"Email send by admin to user for id {emailConfirmareUserId} and titlu {EmailTemplateHumanReadableId}");
            this.unitOfWork.UsersRepository.AddCustomNoteToUser(emailConfirmareUserId, "Trimis Email(" + EmailTemplateHumanReadableId + " " + DateTime.Now.ToShortDateString() + ")");
            this.unitOfWork.Complete();
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult ApartamentReactualizare(int imobilid, string returnUrl)
        {
            this.unitOfWork.AnunturiRepository.ReActualizareAnunt(imobilid);
            this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(imobilid, User.Identity.Name + " react(" + DateTime.Now.ToShortDateString() + ")");
            this.unitOfWork.Complete();
            log.Debug($"Admin {User.Identity.Name} renewed imobil with id {imobilid}");
            return Redirect(returnUrl);
        }

        public ActionResult Maintenance()
        {
            var usedDbSizeInMb = this.unitOfWork.AnunturiRepository.GetDbSize();

            var maxDbSize = this.unitOfWork.SystemSettingsRepository.SystemSettings.MaxDbSize;

            var usedPercentage = (int)((double)usedDbSizeInMb / maxDbSize * 100);

            MaintenanceData maintenanceData = new MaintenanceData();
            maintenanceData.UsedDbInMb = usedDbSizeInMb;
            maintenanceData.MaxDbSizeInMb = maxDbSize;

            return this.View(maintenanceData);
        }

        public ActionResult DeleteLogsOlderThanDate(string logDate1)
        {
            int number = this.unitOfWork.LogsRepository.DeleteLogsOltherThanDate(DateTime.Parse(logDate1, dateFormat));

            TempData["Message"] = number + " Logs deleted successfully";
            return RedirectToAction("Maintenance");
        }

        public ActionResult DeleteAuditTrailsOlderThanDate(string logDate2)
        {
            int number = this.unitOfWork.AuditTrailRepository.DeleteAuditTrailsOltherThanDate(DateTime.Parse(logDate2, dateFormat));

            TempData["Message"] = number + " Audit Trails deleted successfully";
            return RedirectToAction("Maintenance");
        }

        public ActionResult DeletePozeForAnunturiOlderThanDate(string logDate2)
        {
            var number = this.unitOfWork.AnunturiRepository.DeletePozeForExpiredAnunturiOlderThanDate(DateTime.Parse(logDate2, dateFormat));
            this.unitOfWork.Complete();

            TempData["Message"] = $"Maintenance... {number.Item1} Anunturi cleared for secondary Photos successfully, Total {number.Item2} photos";
            return RedirectToAction("Maintenance");
        }

        public ActionResult GetNumberDeletePozeForAnunturiOlderThanDate(string date)
        {
            var number = this.unitOfWork.AnunturiRepository.GetNumberDeletePozeForAnunturiOlderThanDate(DateTime.Parse(date, dateFormat));

            return Content($"{number.Item1} Inactive Anunturi with secondary Photos, Total {number.Item2} photos");
        }

        public ActionResult DeleteAnunturiVechiBulk(string logDate2)
        {
            int number = this.unitOfWork.AnunturiRepository.DeleteAnunturiVechiBulk(DateTime.Parse(logDate2, dateFormat), hostingEnvironment.WebRootPath);
            this.unitOfWork.Complete();

            TempData["Message"] = $"Maintenance... {number} Expired Anunturi deleted";
            return RedirectToAction("Maintenance");
        }

        public ActionResult GetNumberDeleteAnunturiVechiBulk(string date)
        {
            int number = this.unitOfWork.AnunturiRepository.GetNumberDeleteAnunturiVechiBulk(DateTime.Parse(date, dateFormat));

            return Content($"{number} Expired Anunturi until date");
        }

        //public ActionResult ParseExternalUrl(string externalUrl)
        //{
        //    ExternalSourceAnunt externalSourceAnunt = null;
        //    var anuntRelatedlink = this.unitOfWork.AnunturiRepository.GetImobilRelatedToExternalLink(externalUrl);
        //    if (anuntRelatedlink == null)
        //    {
        //        externalSourceAnunt = this.externalSiteContentParser.GetParsedContent(externalUrl);
        //        externalSourceAnunt.LinkExtern = externalUrl;
        //    }
        //    else
        //    {
        //        externalSourceAnunt = new ExternalSourceAnunt() { EroareParsare = "Linkul este atribuit unui anunt anterior! A fost adaugat un anunt deja cu linkul specificat." };
        //    }
        //    return this.Json(externalSourceAnunt);
        //}

        public ActionResult ClearPhotosExceptFirst(int anuntId, string returnUrl)
        {
            this.unitOfWork.AnunturiRepository.ClearPhotosExceptFirst(anuntId);
            this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(anuntId, "Removed photos except first(" + DateTime.Now.ToShortDateString() + ")");
            this.unitOfWork.Complete();
            log.Debug($"Admin {User.Identity.Name} cleared all photos except first for anunt with id {anuntId}");
            return Redirect(returnUrl);
        }

        public ActionResult SchimbaStareMultipleAnunturi(string stareNoua, string ids, string returnUrl)
        {
            int numarAnunturi = 0;
            if (ids != string.Empty)
            {
                numarAnunturi = ids.Split(',').Count();
            }
            log.Debug($"Admin started updating imobil state for a list of selected anunturi to {stareNoua}, numar de id vizate {numarAnunturi}, adica {ids}");
            foreach (var id in ids.Split(','))
            {
                if (id != string.Empty)
                {
                    this.unitOfWork.AnunturiRepository.ChangeImobilState(Int32.Parse(id), stareNoua.EnumParse<StareAprobare>());
                    this.unitOfWork.AnunturiRepository.AddCustomNoteToImobil(Int32.Parse(id), User.Identity.Name + " multiple updt to " + stareNoua + " (" + DateTime.Now.ToShortDateString() + ")");
                    this.unitOfWork.Complete();
                    log.Debug($"Changed anunt with id {id} to {stareNoua}");
                }
            }

            log.Debug($"S-a updatat lista cu anunturi cu succes");
            return this.Redirect(returnUrl.Replace("&amp;", "&"));
        }

        public ActionResult MassEmailMessage()
        {
            MassEmailData massEmailData = new MassEmailData();
            massEmailData.EmailTemplates = this.emailManagerService.GetAllEmailTemplates().Where(x => x.EmailTemplateCategory == EmailTemplateCategory.MassEmail).ToList();
            return this.View(massEmailData);
        }

        //[ValidateInput(false)]
        public ActionResult SendMassEmailMessage(string listaEmails, string emailTemplateHumanReadableId)
        {
            if (string.IsNullOrEmpty(listaEmails))
            {
                TempData["ErrorMessage"] = "listaEmails is null or empty";
                log.Warn($"listaEmails is null or empty");
                return RedirectToAction("MassEmailMessage");
            }

            if (string.IsNullOrEmpty(emailTemplateHumanReadableId))
            {
                TempData["ErrorMessage"] = "emailTemplateHumanReadableId is null or empty";
                log.Warn($"emailTemplateHumanReadableId is null or empty");
                return RedirectToAction("MassEmailMessage");
            }

            List<string> emails = new List<string>();
            foreach (var email in listaEmails.Replace(" ", "").Split(',').Where(x => !string.IsNullOrEmpty(x)))
            {
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!regex.Match(email).Success)
                {
                    TempData["ErrorMessage"] = "Invalid email specified " + email;
                    log.Warn($"Invalid email specified {email}");
                    return RedirectToAction("MassEmailMessage");
                }
                emails.Add(email);
            }

            foreach (var email in emails)
            {
                this.emailManagerService.MassEmailSendEmail(email, emailTemplateHumanReadableId);
            }

            TempData["Message"] = "Emailurile au fost trimise cu succes";
            log.Debug($"Emailurile au fost trimise cu succes la lista {listaEmails} si categoria {emailTemplateHumanReadableId}");

            //this.emailManagerService.MassEmailSendEmail()
            return RedirectToAction("MassEmailMessage");
        }

        [HttpGet]
        public ActionResult SearchAnuntByTitle(string titluAnunt)
        {
            var anunt = this.unitOfWork.AnunturiRepository.GetAnuntByTitle(titluAnunt);

            if (anunt != null)
            {
                return this.RedirectToAction("AnuntEditare", "Administrare", new { id = anunt.Id });
            }
            else
            {
                TempData["WarningMessage"] = "Nu s-a gasit anunt cu titlul " + titluAnunt;
                return Redirect("Index");
            }
        }

        [HttpPost]
        public ActionResult GetAllUsersFromDBCommaSeparated()
        {
            var userProfileList = string.Empty;
            var userProfiles = this.unitOfWork.UsersRepository.GetUserProfiles();

            foreach (var userProfile in userProfiles)
            {
                userProfileList += userProfile.Email + ",";
            }

            return this.Content(userProfileList);
        }

        public ActionResult GetImagesUrlList(string url)
        {
            var urlList = this.externalSiteContentParser.GetPicturesUrlList(url);

            //https://stackoverflow.com/questions/38578463/asp-net-core-the-name-jsonrequestbehavior-does-not-exist-in-the-current-cont
            return this.Json(urlList);
        }

        public ActionResult UploadExternalSiteImages(string link, int idanunt)
        {
            var urlList = this.externalSiteContentParser.GetPicturesUrlList(link);
            //foreach (var imageUrl in urlList)
            //{
            //    //http://stackoverflow.com/questions/1110246/how-do-i-programatically-save-an-image-from-a-url
            //    var imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);

            //    //put useragent because olx is detecting bot
            //    imageRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
            //    WebResponse imageResponse = imageRequest.GetResponse();

            //    Stream responseStream = imageResponse.GetResponseStream();

            //    IFormFile[] IFormFiles = new IFormFile[1];
            //    IFormFiles[0] = new MemoryFile(responseStream, "contentType", "name");

            //    this.unitOfWork.AnunturiRepository.AddImages(idanunt, IFormFiles);
            //    this.unitOfWork.Complete();

            //    responseStream.Close();
            //    imageResponse.Close();
            //}
            return RedirectToAction("AnuntEditare", "Administrare", new RouteValueDictionary(new Dictionary<string, object>() { { "id", idanunt } }));
        }

        public ActionResult StergeAnunt(int id)
        {
            try
            {
                this.unitOfWork.AnunturiRepository.DeleteImobil(id, hostingEnvironment.WebRootPath);
                this.unitOfWork.Complete();
                log.Debug($"Deleted imobil with id {id} by administrator from anunt page {User.Identity.Name}");

                TempData["Message"] = "Anunt sters cu succes";
            }
            catch (Exception e)
            {
                TempData["WarningMessage"] = "Eroare la stergerea anuntului cu id " + id;
                log.Error($"Eroare la stergerea anuntului cu id {id} by administrator from anunt page {User.Identity.Name}, eroare: {e.Message}");
            }

            return RedirectToAction("Index", "SuperAdmin");
        }
    }

    //public class MemoryFile : IFormFile
    //{
    //    Stream stream;
    //    string contentType;
    //    string fileName;

    //    public MemoryFile(Stream stream, string contentType, string fileName)
    //    {
    //        this.stream = stream;
    //        this.contentType = contentType;
    //        this.fileName = fileName;
    //    }

    //    public override int ContentLength
    //    {
    //        get { return (int)stream.Length; }
    //    }

    //    public override string ContentType
    //    {
    //        get { return contentType; }
    //    }

    //    public override string FileName
    //    {
    //        get { return fileName; }
    //    }

    //    public override Stream InputStream
    //    {
    //        get { return stream; }
    //    }

    //    public override void SaveAs(string filename)
    //    {
    //        using (var file = File.Open(filename, FileMode.CreateNew))
    //            stream.CopyTo(file);
    //    }
    //}
}
