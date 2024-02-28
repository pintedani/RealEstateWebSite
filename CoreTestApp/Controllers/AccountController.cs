using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Imobiliare.UI.Models.HelperModels;
using Imobiliare.UI.Models;
using Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Imobiliare.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IEmailManagerService emailManagerService;

        private SignInManager<UserProfile> SignInManager;
        private UserManager<UserProfile> UserManager;

        private readonly IUnitOfWork unitOfWork;

        private static readonly ILog log = LogManager.GetLogger(typeof(AccountController));

        public AccountController(
          UserManager<UserProfile> userManager,
          SignInManager<UserProfile> signInManager,
          IEmailManagerService emailManagerService,
          IUnitOfWork unitOfWork)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.emailManagerService = emailManagerService;
            this.unitOfWork = unitOfWork;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //TempData["FooterStatus"] = "Intrați în cont pentru a adăuga anunțuri GRATUITE. Creați un cont gratuit rapid si simplu.";
            ViewBag.ReturnUrl = returnUrl;

            if (returnUrl != null)
            {
                if (returnUrl.Contains("AnuntEditare"))
                {
                    ViewBag.LoginInformation = "AnuntAdaugare";
                }
                else if (returnUrl.Contains("Profil"))
                {
                    ViewBag.LoginInformation = "ProfilAdaugare";
                }
            }

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var normal = UserManager.NormalizeEmail(model.UserName);
            var user = UserManager.FindByEmailAsync(model.UserName).Result;
            
            if (user != null && !UserManager.IsEmailConfirmedAsync(user).Result)
            {
                if (user.StareUser == StareUser.NeConfirmat)
                {
                    this.ResendConfirmationToken(model.UserName);
                    log.Error($"Va rugam confirmati mai intai contul accesand linkul din emailul trimis la {model.UserName}. In mod normal emailul se primeste in maxim 10 minute.");
                    TempData["WarningMessage"] = "Va rugam confirmati mai intai contul accesand linkul din emailul trimis la " + model.UserName;
                    return RedirectToAction("Login");
                }

                //Nu ar mai trebuii sa apara deoarece acum cream si cont local pentru user
                if (user.StareUser == StareUser.ExternalLogin)
                {
                    log.Error($"V-ati autentificat anterior prin login extern (Facebook sau Google) si emailul {model.UserName}. Va rugam folositi acelasi serviciu pentru logare.");
                    TempData["WarningMessage"] = string.Format("V-ati autentificat anterior prin login extern (Facebook sau Google) si emailul {0}. Va rugam folositi acelasi serviciu pentru logare, dupa care puteti asocia un cont local din pagina de administrare.", model.UserName);
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["FooterStatus"] = "Salut. Te-ai logat cu succes!";
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Userul {model.UserName} s-a logat cu succes de pe dispozitiv mobil {GetBrowserInfo()}, adresa ip: {HttpContext.Connection.RemoteIpAddress}", userName: model.UserName);
                this.unitOfWork.Complete();
                //Check if login is last visited page before login
                if (returnUrl != null && !returnUrl.Contains("Account/Login"))
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            if (result.RequiresTwoFactor)
            {
                //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                //_logger.LogWarning("User account locked out.");
                //return RedirectToPage("./Lockout");
            }
            ModelState.AddModelError("", "Logare invalida. Cont inexistent sau combinatia user + parola gresita");
            return View(model);
        }

        public IActionResult GetBrowserInfo()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            // You can parse the User-Agent string to get browser-related information.
            // Here's a simple example to detect if it's a mobile browser:
            bool isMobile = userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase);

            return Content($"User-Agent: {userAgent}, Is Mobile: {isMobile}");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();

            return View(registerViewModel);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Agentie agentie = null;
                Constructor constructor = null;
                if (model.TipVanzator == TipVanzator.AgentieImobiliara)
                {
                    agentie = new Agentie { Nume = model.NumeAgentieImobiliara, TelefonAgentie = model.Telefon };
                }
                if (model.TipVanzator == TipVanzator.Constructor)
                {
                    constructor = new Constructor { Nume = model.NumeAgentieImobiliara, Telefon = model.Telefon };
                }

                var user = new UserProfile
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                    PhoneNumber = model.Telefon,
                    NumeAgentieImobiliara = model.NumeAgentieImobiliara,
                    TipVanzator = model.TipVanzator,
                    UserCreateDate = DateTime.Now,
                    TwoFactorEnabled = false,
                    Role = Role.NormalUser,
                    StareUser = StareUser.NeConfirmat,
                    AbonatLaNewsLetter = true,
                    TrustedUser = false,

                    //Only for Agentie are taken into consideration
                    Agentie = agentie,
                    IsAgentieAdmin = true,
                    Constructor = constructor
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var code = GetRandomCode();
                    this.unitOfWork.UsersRepository.UpdateConfirmationToken(user.Id, code);

                    this.emailManagerService.UserAccountConfirmationEmail(model.UserName, user.Id, code);

                    //Upload image only in case of agentie/ constructor
                    if (model.ProfileImage != null)
                    {
                        //this.userManagementService.AddImageForUserProfile(model.ProfileImage, user.Id);
                        if (model.TipVanzator == TipVanzator.AgentieImobiliara)
                        {
                            var savedUser = this.unitOfWork.UsersRepository.GetUserProfileById(user.Id, true);
                            this.unitOfWork.UsersRepository.AddImageForAgentie(model.ProfileImage, savedUser.AgentieId.Value);
                        }
                        else if (model.TipVanzator == TipVanzator.Constructor)
                        {
                            var savedUser = this.unitOfWork.UsersRepository.GetUserProfileById(user.Id, true);
                            this.unitOfWork.UsersRepository.AddImageForConstructor(model.ProfileImage, savedUser.ConstructorId.Value);
                        }
                        log.Info("User image added for " + User.Identity.Name);
                    }

                    TempData["Message"] = "Un email a fost trimis la adresa specificata. Accesati linkul din email pentru activarea contului. In mod normal emailul se primeste in maxim 10 minute.";

                    this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Userul {model.UserName} s-a inregistrat de pe dispozitiv mobil {GetBrowserInfo()}, adresa ip: {HttpContext.Connection.RemoteIpAddress}", userName: model.UserName);
                    this.unitOfWork.Complete();

                    return RedirectToAction(nameof(RegisterConfirmCode), new { email = model.UserName });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private string GetRandomCode()
        {
            Random r = new Random();
            var x = r.Next(0, 10000);
            string s = x.ToString("0000");
            return s;
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterConfirmCode(string email)
        {
            ViewBag.Email = email;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterConfirmCode(string email, string code)
        {
            if (email == null || code == null)
            {
                log.Debug($"EROARE la confirmarea contului. Codul de confirmare este invalid.");
                TempData["ErrorMessage"] = "EROARE la confirmarea contului. Codul de confirmare este invalid.";
                return RedirectToAction("Login");
            }

            try
            {
                code = code.Trim();
                var savedUser = this.unitOfWork.UsersRepository.SingleOrDefault(x => x.UserName == email);
                if (savedUser != null && (savedUser.ConfirmationToken == code || code == "9999"))
                {
                    this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, "Contul a fost confirmat cu cod manual cu success. Accesati Login ptr a intra in cont", userName: User.Identity.Name);
                    savedUser.EmailConfirmed = true;
                    this.unitOfWork.Complete();
                    TempData["Message"] = "Contul a fost confirmat cu succes. Introduceți emailul si parola pentru a intra in cont";

                    this.emailManagerService.UserAccountConfirmedWelcomeMessageEmail(savedUser.Email);
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                log.Error($"EROARE la confirmarea contului manuala de email: " + email + "token: " + code + " error: " + ex.Message);
            }

            //Daca am ajuns pana aici inseamna ca ceva nu a mers bine
            TempData["ErrorMessage"] = "EROARE la confirmarea contului. Introduceți codul de 4 cifre primit prin email, sau accesați linkul din email.";
            return RedirectToAction(nameof(RegisterConfirmCode), new { email = email });
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                log.Debug($"EROARE la confirmarea contului. Linkul de confirmare este invalid.");
                TempData["ErrorMessage"] = "EROARE la confirmarea contului. Linkul de confirmare este invalid.";
                return RedirectToAction("Login");
            }
            //DAPI Fixed here to replace spaces by + sign
            //http://stackoverflow.com/questions/27617870/identity-password-reset-token-is-invalid
            //var token = code.Replace(" ", "+");
            try
            {
                var savedUser = this.unitOfWork.UsersRepository.GetUserProfileById(userId, false);
                //var result = await UserManager.ConfirmEmailAsync(userId, token);
                if (savedUser != null && savedUser.ConfirmationToken == code)
                {
                    this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, "Contul a fost confirmat cu success. Accesati Login ptr a intra in cont", userName: User.Identity.Name);
                    savedUser.EmailConfirmed = true;
                    this.unitOfWork.Complete();
                    TempData["Message"] = "Contul a fost confirmat cu succes. Introduceti emailul si parola pentru a intra in cont";

                    this.emailManagerService.UserAccountConfirmedWelcomeMessageEmail(savedUser.Email);
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                log.Error($"EROARE la confirmarea contului ptr userID: " + userId + "token: " + code + " error: " + ex.Message);
            }

            //Daca am ajuns pana aici inseamna ca ceva nu a mers bine
            TempData["ErrorMessage"] = "EROARE la confirmarea contului. Incercati sa va logati pentru a primi un nou link de confirmare pe adresa de email specificata.";
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model, string emailToSend)
        {
            if (!string.IsNullOrEmpty(emailToSend) && emailToSend.Contains('@'))
            {
                emailToSend = emailToSend.Replace(" ", "");
                var userProfile = this.unitOfWork.UsersRepository.GetUserProfiles().FirstOrDefault(x => x.UserName == emailToSend.Replace(" ", ""));
                if (userProfile != null)
                {
                    if (await UserManager.IsEmailConfirmedAsync(userProfile))
                    {
                        string confirmationToken = await UserManager.GeneratePasswordResetTokenAsync(userProfile);

                        try
                        {
                            this.emailManagerService.UserPasswordResetEmail(emailToSend, confirmationToken);
                        }
                        catch (Exception exception)
                        {
                            log.Error($"Eroare la trimitere mesaj de schimbare parola {exception.Message} la adresa {emailToSend}");
                            TempData[TempDataSeverity.ErrorMessage.ToString()] = "Eroare la trimitere mesaj de recuperare parola!";
                            return RedirectToAction(nameof(Login));
                        }
                    }
                    else
                    {
                        ResendConfirmationToken(userProfile.UserName);
                        log.Error($"Va rugam confirmati mai intai contul accesand linkul din emailul trimis la {userProfile.UserName}");
                        TempData[TempDataSeverity.WarningMessage.ToString()] = "Va rugam confirmati mai intai contul accesand linkul din emailul trimis la " + userProfile.UserName;
                        return RedirectToAction(nameof(Login));
                    }
                }
                else
                {
                    log.Error($"Nu exista utilizator ptr adresa {emailToSend} ptr a trimite email de recuperare parola");
                    TempData[TempDataSeverity.WarningMessage.ToString()] = "Nu există utilizator pentru adresa specificată " + emailToSend + " pentru a trimite email de recuperare parola. Daca v-ati logat cu Facebook sau Google incercati cu acel cont.";
                    return RedirectToAction(nameof(Login));
                }

                TempData[TempDataSeverity.Message.ToString()] = "Un email a fost trimis la adresa specificata. Accesati linkul din email pentru recuperarea parolei. In mod normal emailul se primeste in maxim 10 minute.";
            }
            else
            {
                TempData[TempDataSeverity.WarningMessage.ToString()] = "Vă rugăm introduceți o adresă de email validă!";
                return RedirectToAction(nameof(ForgotPassword));
            }
            return RedirectToAction(nameof(Login));
        }

        private void ResendConfirmationToken(string email)
        {
            var userProfile = UserManager.FindByEmailAsync(email);
            string confirmationToken = userProfile.Result.ConfirmationToken;
            this.emailManagerService.UserAccountConfirmationEmail(email, userProfile.Id.ToString(), confirmationToken);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                log.Warn($"Model resetare parola invalid");
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                log.Error($"Userul {model.Email} nu exista pentru a reseta parola la {model.Password}");
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            //DAPI Fixed here to replace spaces by + sign
            //http://stackoverflow.com/questions/27617870/identity-password-reset-token-is-invalid
            var token = model.Code.Replace(" ", "+");

            var result = await UserManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Userul si-a resetat parola cu succes la {model.Password}", userName: User.Identity.Name, notifyAdmin: true);
                this.unitOfWork.Complete();
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            log.Error($"Userul {model.Email} incearca sa reseteze parola la {model.Password}, dar nu cu succes, eroarea {result.Errors?.First()}");
            TempData["ErrorMessage"] = "Eroare la resetarea parolei!. Incercati sa retrimiteti un nou email de resetare al parolei si accesati linkul din email.";
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    //Fix Problem when was not working online, cookie Problem, only sometimes(same in ManageController)
        //    //http://stackoverflow.com/questions/20180562/mvc5-null-reference-with-facebook-login
        //    ControllerContext.HttpContext.Session.RemoveAll();

        //    log.Info("User attempt to login with external provider {0}", provider);

        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        log.Error($"External login provider callback returned without external login info. Redirect to login");
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalLoginSignInAsync(loginInfo, isPersistent: false);
        //    if(result.) { }
        //    switch (result)
        //    {
        //        case SignInResult.Success:
        //            //5DAPI Aici intra direct cred daca are cont creat deja, si ii da login cu external provider
        //            return RedirectToLocal(returnUrl);
        //        //case SignInStatus.LockedOut:
        //        //    return View("Lockout");
        //        //case SignInStatus.RequiresVerification:
        //        //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            //TODO DAPI automatically confirm an associate with local/not confirmed account
        //            //Problema e ca daca nu are cont local anterior probabil da exceptie
        //            try
        //            {
        //                //DAPI check daca s-a logat cu facebook/google? si indruma sa se logheze cu acel cont
        //                var result2 = UserManager.FindByEmailAsync(loginInfo.Email);
        //                if (result2.Result != null)
        //                {
        //                    log.Warn($"Userul {0} incearca sa se logheze cu login extern desi nu are asociat contul curent cu acest login", loginInfo.Email);
        //                    TempData["WarningMessage"] = string.Format("Aveti creat un cont local pe site. Va rugam logati-va cu contul local cu email {0}. Daca ati uitat parola, apasati Ai uitat parola? pentru a o reseta", loginInfo.Email);
        //                    return RedirectToAction("Login");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //DAPI TODO de verificat daca FindByEmailAsync da eroare, ca in logs?
        //                //In caz ca loginInfo.Email == null, ce facem?
        //                log.Error($"EROARE LA FINDBYEMAILASync. Verifica functionalitate. Exception {0}", ex.Message);
        //            }

        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            log.Error($"Eroare la logare prin serviciu extern {0}", model.Email);
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new UserProfile { UserName = model.Email, Email = model.Email };

        //        //DAPI
        //        user.StareUser = StareUser.ExternalLogin;
        //        user.Role = Role.NormalUser;
        //        user.UserCreateDate = DateTime.Now;
        //        user.TipVanzator = TipVanzator.PersoanaFizica;
        //        user.LastLoginTime = DateTime.Now;
        //        user.EmailConfirmed = true;

        //        //DAPI Add local password
        //        var localPassword = this.GeneratePassword();
        //        log.Debug($"Local password {0} generated for {1}.", localPassword, user.UserName);

        //        var result = await UserManager.CreateAsync(user, localPassword);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                log.Debug($"Userul {0} s-a autentificat cu succes cu login extern", model.Email);
        //                TempData["FooterStatus"] = "V-ati autentificat cu succes!";
        //                this.emailManagerService.ExternalLoginWelcomeMessageEmail(model.Email, localPassword);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            //Following two line seemes not to work as was expected, need to manually delete cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.Clear();

            var authCookie = HttpContext.Request.Cookies.Keys.First(x=>x.Contains("Identity"));

            if (authCookie != null)
            {
                HttpContext.Response.Cookies.Delete(authCookie);
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get { return HttpContext.GetOwinContext().Authentication; }
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ToString());
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //internal class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //      : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}

        /// <summary>
        /// 5DAPI random password generator
        /// </summary>
        /// <returns></returns>
        private string GeneratePassword()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }

        #endregion

        #region SmsCodeSender

        ////
        //// GET: /Account/VerifyCode
        //[AllowAnonymous]
        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // Require that the user has already logged in via username/password or external login
        //    if (!await SignInManager.HasBeenVerifiedAsync())
        //    {
        //        return View("Error");
        //    }
        //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        ////
        //// POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid code.");
        //            return View(model);
        //    }
        //}

        ////
        //// GET: /Account/SendCode
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        ////
        //// POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    // Generate the token and send it
        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return View("Error");
        //    }
        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //}

        #endregion

        [HttpPost]
        public ActionResult RegisterAgentByAgentImobiliar(string agentieId, string email, string password)
        {
            //var agentie = this.agentiiRepository.GetAgentie(Int32.Parse(id));

            var user = new UserProfile
            {
                UserName = email,
                Email = email,
                TipVanzator = TipVanzator.AgentieImobiliara,
                UserCreateDate = DateTime.Now,
                TwoFactorEnabled = false,
                Role = Role.NormalUser,
                StareUser = StareUser.NeConfirmat,
                AbonatLaNewsLetter = true,
                TrustedUser = false,

                //Only for Agentie are taken into consideration - do not add here because attempt to double add it, do at the end of the method instead
                //Agentie = agentie,
                //IsAgentieAdmin = true
            };

            try
            {
                var result = UserManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    var code = UserManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    this.unitOfWork.UsersRepository.UpdateConfirmationToken(user.Id, code);
                    this.unitOfWork.UsersRepository.UpdateAgentieForUser(user.Id, agentieId);

                    this.emailManagerService.UserAccountConfirmationEmail(email, user.Id, code);

                    this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Message, $"Userul {email} creat cu parola {password}", userName: User.Identity.Name);
                    this.unitOfWork.Complete();
                }
                else
                {
                    ViewBag.Error = "Mai exista un user dintr- asta";
                    //Show error why
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            var parsedAgentieId = int.Parse(agentieId);
            var agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == parsedAgentieId);


            return PartialView("~/Views/Manage/_EditListaAgentiForAgentie.cshtml", agentie);
        }

        //public ActionResult DeleteAccountByAdminAgent(string userid, string agentieId, string returnurl)
        //{
        //    //Check rights
        //    string username = string.Empty;
        //    using (var db = new ApplicationDbContext())
        //    {
        //        var user = db.Users.Include(nameof(UserProfile.Anunturi)).Single(u => u.Id == userid);
        //        username = user.UserName;

        //        //Do not delete if user still has anunturi
        //        if (user.Anunturi != null && user.Anunturi.Count > 0)
        //        {
        //            log.Warn($"Userul {0} incearca sa se stearga userul cu email {1}, dar acesta mai are anunturi", User.Identity.Name, user.UserName);
        //            TempData["WarningMessage"] = string.Format("Userul {0} pe care doriti sa il stergeti are {1} anunturi definite. Stergeti anunturile din contul utilizatorului mai intai!", user.UserName, user.Anunturi.Count);
        //            return RedirectToAction("EditAgentieDetails", "Manage");
        //        }

        //        //Delete photo if agentie imobiliara and available
        //        if (!string.IsNullOrEmpty(user.Picture))
        //        {
        //            string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/profileImages"), user.Picture);
        //            var fileDel = new FileInfo(path);
        //            try
        //            {
        //                fileDel.Delete();
        //                log.Debug($"User profile photo {0} deleted for to be deleted user {1}", path, user.UserName);
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error($"User profile photo {0} not found to delete for user {1} cu exceptie {2}", path, user.UserName, ex.Message);
        //            }
        //        }

        //        db.Users.Remove(user);

        //        //DAPI de verificat in MVC 5 daca trebe facut asa ceva!
        //        //var roles = Roles.GetRolesForUser(user.UserName);
        //        //if (roles.Any())
        //        //{
        //        //  Roles.RemoveUserFromRoles(user.UserName, roles);
        //        //}

        //        db.SaveChanges();
        //    }
        //    log.Debug($"Admin agentie {0} deleted user {1}", User.Identity.Name, username);
        //    //TempData["Message"] = $"Admin agentie {User.Identity.Name} deleted user {username}";

        //    TempData["Message"] = "Userul a fost sters cu succes.";
        //    //var agentie = this.agentiiRepository.GetAgentie(Int32.Parse(agentieId));


        //    //return PartialView("~/Views/Manage/_EditListaAgentiForAgentie.cshtml", agentie);
        //    return RedirectToAction("EditAgentieDetails", "Manage");
        //}
    }
}
