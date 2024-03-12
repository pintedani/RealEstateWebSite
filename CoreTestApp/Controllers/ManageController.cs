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
using System.Security.Claims;
using System.Net;
using AutoMapper;
using Microsoft.Extensions.Hosting.Internal;

namespace Imobiliare.UI.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private SignInManager<UserProfile> SignInManager;
        private UserManager<UserProfile> UserManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        private readonly IUnitOfWork unitOfWork;

        private static readonly ILog log = LogManager.GetLogger(typeof(ManageController));

        public ManageController(
          SignInManager<UserProfile> signInManager,
          UserManager<UserProfile> userManager,
          IUnitOfWork unitOfWork,
          IWebHostEnvironment hostingEnvironment)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
              message == ManageMessageId.ChangePasswordSuccess
                ? "Parola a fost schimbata."
                : message == ManageMessageId.SetPasswordSuccess
                  ? "Parola a fost setata."
                  : message == ManageMessageId.SetTwoFactorSuccess
                    ? "Your two-factor authentication provider has been set."
                    : message == ManageMessageId.Error
                      ? "A aparut o eroare."
                      : message == ManageMessageId.AddPhoneSuccess
                        ? "Your phone number was added."
                        : message == ManageMessageId.RemovePhoneSuccess
                          ? "Your phone number was removed."
            : "";

            var user = GetCurrentUserAsync();
            var userId = await UserManager.GetUserIdAsync(user);
            var userLogins = await UserManager.GetLoginsAsync(user);
            //var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = HasPassword() || userLogins.Count > 1;
            var model = new ManageAccountViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(user),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(user),
                Logins = await UserManager.GetLoginsAsync(user),
                //BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
                //OtherLogins = otherLogins
            };
            return View(model);
        }

        private UserProfile GetCurrentUserAsync()
        {
            //var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            //var user = await UserManager.FindByIdAsync(userIdClaim.Value);
            //var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            //TODO remove await?

            var user = UserManager.FindByEmailAsync(User.Identity.Name).Result;
            return user;
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var user = GetCurrentUserAsync();
            var userId = await UserManager.GetUserIdAsync(user);
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (result.Succeeded)
            {
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = UserManager.FindByEmailAsync(User.Identity.Name).Result;
            
            ViewBag.HasLocalPassword = user != null;
            //ViewBag.ReturnUrl = Url.Action("Index", "Manage");
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        log.Debug($"Password changed successfully for user {user.UserName}, old: {model.OldPassword}, new: {model.NewPassword}");
                        TempData["Message"] = "Parola a fost schimbata cu succes";
                        return RedirectToAction("ChangePassword");
                    }
                    else
                    {
                        log.Debug("Password change failed for " + User.Identity.Name);
                        ModelState.AddModelError("", "Parola curenta este incorecta sau invalida.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmailAsync(User.Identity.Name).Result;
                var result = await UserManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    if (user != null)
                    {
                        //5DAPI pune user confirmat aici, role, etc
                        user.Role = Role.NormalUser;
                        user.StareUser = StareUser.Confirmat;
                        user.UserCreateDate = DateTime.Now;
                        user.LastLoginTime = DateTime.Now;
                        //No more external provider ? Daca creeaza oricum cont local?
                        user.StareUser = StareUser.Confirmat;
                        user.AbonatLaNewsLetter = true;
                        this.unitOfWork.UsersRepository.UpdateUserProfile(user);
                        this.unitOfWork.Complete();
                        log.Debug($"Userul {user.Email} si-a creat prola locala, astfel ca s-a modificat la user local");

                        await SignInManager.SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.RemoveLoginSuccess
              ? "Loginul extern a fost sters."
              : message == ManageMessageId.Error
                ? "A aparut o eroare."
                : "";

            var user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(user);
            //var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins
                //OtherLogins = otherLogins
            });
        }

        public async Task<ActionResult> DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteAccountConfirmed()
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);

            if (user.Role == Role.Administrator)
            {
                log.Error($"Admin tried to delete it's account! Not accepted");
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            //Log off
            //AuthenticationManager.SignOut(CookieAuthenticationDefaults.CookiePrefix);
            var authCookie = HttpContext.Request.Cookies.Keys.First(x => x.Contains("Identity"));

            if (authCookie != null)
            {
                HttpContext.Response.Cookies.Delete(authCookie);
            }

            var anunturi = this.unitOfWork.AnunturiRepository.Find(x => x.UserId == user.Id);

            log.Warn($"Attempting to delete anunturi for user {user.UserName}, total anunturi: {anunturi.Count}");
            foreach (var anunt in anunturi)
            {
                this.unitOfWork.AnunturiRepository.DeleteImobil(anunt.Id, hostingEnvironment.WebRootPath);
            }

            this.unitOfWork.UsersRepository.DeleteUser(user.Id);
            this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Warning, "ACOUNT DELETED by user {0}", user.UserName, notifyAdmin: true);
            this.unitOfWork.Complete();
            log.Warn($"ACOUNT DELETED by user {user.UserName}");
            TempData["Message"] = $"Contul a fost sters cu succes!";
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            //Fix Problem when was not working online, only sometimes(same in AccountController)
            //http://stackoverflow.com/questions/20180562/mvc5-null-reference-with-facebook-login
            //ControllerContext.HttpContext.Session.RemoveAll();

            //log.Debug($"User attempt to attach new external login provider {0}", provider);

            //// Request a redirect to the external login provider to link a login for the current user
            //return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
            return null;
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            //var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            ////Check why this returns null on server only? On localhost it works
            //var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, user);
            //if (loginInfo == null)
            //{
            //    log.Error($"External authentication callback returned with error for id {0}", user.UserName);
            //    return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            //}
            //var result = await UserManager.AddLoginAsync(user, loginInfo.Login);
            //return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

            return null;
        }

        public ActionResult EditUserDetails()
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);

            switch (user.TipVanzator)
            {
                case TipVanzator.PersoanaFizica:
                    return RedirectToAction(nameof(this.EditPersoanaFizicaUserprofile), "Manage", new { userId = user.Id });

                case TipVanzator.AgentieImobiliara:
                    return RedirectToAction(nameof(this.EditAgentieImobiliaraUserprofile), "Manage", new { userId = user.Id });

                case TipVanzator.Constructor:
                    return RedirectToAction(nameof(this.EditConstructorUserprofile), "Manage", new { userId = user.Id });

                default:
                    throw new NotSupportedException("Invalid user type");
            }
        }

        public ActionResult EditPersoanaFizicaUserprofile(string userId)
        {
            var user = this.unitOfWork.UsersRepository.GetUserProfileById(userId, false);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, EditPersoanaFizicaDetailsModel>()).CreateMapper();

            var editPersoanaFizicaUserprofile = mapper.Map<EditPersoanaFizicaDetailsModel>(user);
            //var editPersoanaFizicaUserprofile = new EditPersoanaFizicaDetailsModel();
            return this.View(editPersoanaFizicaUserprofile);
        }

        [HttpPost]
        public ActionResult EditPersoanaFizicaUserprofile(EditPersoanaFizicaDetailsModel editPersoanaFizicaDetailsModel)
        {
            if (ModelState.IsValid)
            {
                var toEditUserProfile = this.unitOfWork.UsersRepository.GetUserProfileById(editPersoanaFizicaDetailsModel.Id, false);

                toEditUserProfile.AbonatLaNewsLetter = editPersoanaFizicaDetailsModel.AbonatLaNewsLetter;
                toEditUserProfile.NumeComplet = editPersoanaFizicaDetailsModel.NumeComplet;
                toEditUserProfile.PhoneNumber = editPersoanaFizicaDetailsModel.PhoneNumber;

                this.unitOfWork.UsersRepository.UpdateUserProfile(toEditUserProfile);
                log.Debug("PF User details edited for user " + User.Identity.Name);

                //If user changed type tip vanzator to AGENTIE in the meantime
                if (editPersoanaFizicaDetailsModel.TipVanzator == TipVanzator.AgentieImobiliara)
                {
                    log.Debug("PF User changed type to agentie {0} " + User.Identity.Name);
                    var agentie = new Agentie { Nume = editPersoanaFizicaDetailsModel.NumeAgentieImobiliara };
                    this.unitOfWork.UsersRepository.AssociateUserProfileWithAgency(toEditUserProfile.Id, agentie, true);

                    //Update agentie association(need to be saved in Db before making association)
                    var user = this.unitOfWork.UsersRepository.GetUserProfileById(editPersoanaFizicaDetailsModel.Id, true);

                    if (editPersoanaFizicaDetailsModel.ProfileImage != null)
                    {
                        //this.unitOfWork.UsersRepository.AddImageForAgentie(editPersoanaFizicaDetailsModel.ProfileImage, user.AgentieId.Value);
                        log.Debug("User image added for " + User.Identity.Name);
                    }
                }

                //If user changed type tip vanzator to CONSTRUCTOR in the meantime
                if (editPersoanaFizicaDetailsModel.TipVanzator == TipVanzator.Constructor)
                {
                    log.Debug("PF User changed type to constructor {0} " + User.Identity.Name);
                    var constructor = new Constructor { Nume = toEditUserProfile.NumeAgentieImobiliara };
                    this.unitOfWork.UsersRepository.AssociateUserProfileWithConstructor(toEditUserProfile.Id, constructor);

                    //Update agentie association(need to be saved in Db before making association)
                    var user = this.unitOfWork.UsersRepository.GetUserProfileById(editPersoanaFizicaDetailsModel.Id, true);

                    if (editPersoanaFizicaDetailsModel.ProfileImage != null)
                    {
                        //this.unitOfWork.UsersRepository.AddImageForConstructor(editPersoanaFizicaDetailsModel.ProfileImage, user.ConstructorId.Value);
                        log.Debug("User image added for " + User.Identity.Name);
                    }
                }

                this.unitOfWork.Complete();

                TempData["Message"] = "Modificarile au fost salvate.";
                return RedirectToAction("EditUserDetails");
            }

            return View(editPersoanaFizicaDetailsModel);
        }

        public ActionResult EditAgentieImobiliaraUserprofile(string userId)
        {
            var user = this.unitOfWork.UsersRepository.GetUserProfileById(userId, true);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, EditAgentImobiliarModel>()).CreateMapper();
            var editAgentieImobiliaraDetailsModel = mapper.Map<EditAgentImobiliarModel>(user);
            //var editPersoanaFizicaUserprofile = new EditPersoanaFizicaDetailsModel();
            return this.View(editAgentieImobiliaraDetailsModel);
        }

        [HttpPost]
        public ActionResult EditAgentieImobiliaraUserprofile(EditAgentImobiliarModel editAgentImobiliarModel)
        {
            if (ModelState.IsValid)
            {
                var toEditUserProfile = this.unitOfWork.UsersRepository.GetUserProfileById(editAgentImobiliarModel.Id, false);

                toEditUserProfile.AbonatLaNewsLetter = editAgentImobiliarModel.AbonatLaNewsLetter;
                toEditUserProfile.NumeComplet = editAgentImobiliarModel.NumeComplet;
                toEditUserProfile.PhoneNumber = editAgentImobiliarModel.PhoneNumber;

                this.unitOfWork.UsersRepository.UpdateUserProfile(toEditUserProfile);
                log.Debug("Agent imobiliar details edited for user " + User.Identity.Name);


                if (editAgentImobiliarModel.ProfileImage != null)
                {
                    this.unitOfWork.UsersRepository.AddImageForUserProfile(editAgentImobiliarModel.ProfileImage, toEditUserProfile.Id);
                    log.Debug("User image added for " + User.Identity.Name);
                }

                this.unitOfWork.Complete();

                TempData["Message"] = "Modificarile au fost salvate.";
                return RedirectToAction("EditUserDetails");
            }

            return View(editAgentImobiliarModel);
        }

        public ActionResult EditAgentieDetails()
        {
            var toEditUserProfile = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);

            var agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == toEditUserProfile.AgentieId.Value);

            var editAgentieImobiliaraModel = new EditAgentieImobiliaraModel() { Agentie = agentie };

            return this.View(editAgentieImobiliaraModel);
        }

        public ActionResult EditAgentieManageAgenti()
        {
            var toEditUserProfile = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);

            var agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == toEditUserProfile.AgentieId.Value);

            var editAgentieImobiliaraModel = new EditAgentieImobiliaraModel() { Agentie = agentie };

            return this.View(editAgentieImobiliaraModel);
        }

        [HttpPost]
        public ActionResult EditAgentieDetails(EditAgentieImobiliaraModel editAgentieImobiliaraModel)
        {
            if (ModelState.IsValid)
            {
                var toEditAgentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == editAgentieImobiliaraModel.Agentie.Id);

                //Check rights

                toEditAgentie.Nume = editAgentieImobiliaraModel.Agentie.Nume;
                toEditAgentie.DescriereAgentie = editAgentieImobiliaraModel.Agentie.DescriereAgentie;
                toEditAgentie.Website = editAgentieImobiliaraModel.Agentie.Website;
                toEditAgentie.TelefonAgentie = editAgentieImobiliaraModel.Agentie.TelefonAgentie;
                if (editAgentieImobiliaraModel.OrasSelect != 0)
                {
                    toEditAgentie.OrasSelect = editAgentieImobiliaraModel.OrasSelect;
                    toEditAgentie.Oras = this.unitOfWork.OrasRepository.Single(x => x.Id == editAgentieImobiliaraModel.OrasSelect);
                }
                else
                {
                    toEditAgentie.OrasSelect = null;
                    toEditAgentie.Oras = null;
                }

                this.unitOfWork.AgentiiRepository.Update(toEditAgentie);
                log.Debug($"Agent {User.Identity.Name} edited agentie details");

                if (editAgentieImobiliaraModel.ProfileImage != null)
                {
                    this.unitOfWork.UsersRepository.AddImageForAgentie(editAgentieImobiliaraModel.ProfileImage, toEditAgentie.Id);
                    log.Debug("Agentie image added for " + toEditAgentie.Nume);
                }

                this.unitOfWork.Complete();

                TempData["Message"] = "Modificarile au fost salvate.";
                return RedirectToAction("EditAgentieDetails");
            }

            //Get related relations to agentie users
            editAgentieImobiliaraModel.Agentie = this.unitOfWork.AgentiiRepository.Single(x => x.Id == editAgentieImobiliaraModel.Agentie.Id);
            return View(editAgentieImobiliaraModel);
        }

        public ActionResult EditConstructorUserprofile(string userId)
        {
            var user = this.unitOfWork.UsersRepository.GetUserProfileById(userId, true);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, EditConstructorImobiliarModel>()).CreateMapper();
            var editConstructorDetailsModel = mapper.Map<EditConstructorImobiliarModel>(user);
            editConstructorDetailsModel.ConstructorId = user.ConstructorId.Value;
            editConstructorDetailsModel.ConstructorNume = user.Constructor.Nume;
            editConstructorDetailsModel.ConstructorDescriere = user.Constructor.Descriere;
            editConstructorDetailsModel.ConstructorWebsite = user.Constructor.Website;
            editConstructorDetailsModel.ConstructorTelefon = user.Constructor.Telefon;
            editConstructorDetailsModel.ConstructorPoza = user.Constructor.Poza;

            //Mapper get manual the props for constructor company
            return this.View(editConstructorDetailsModel);
        }

        [HttpPost]
        public ActionResult EditConstructorUserprofile(EditConstructorImobiliarModel editConstructorImobiliarModel)
        {
            if (ModelState.IsValid)
            {
                var toEditUserProfile = this.unitOfWork.UsersRepository.GetUserProfileById(editConstructorImobiliarModel.Id, false);

                toEditUserProfile.AbonatLaNewsLetter = editConstructorImobiliarModel.AbonatLaNewsLetter;
                toEditUserProfile.NumeComplet = editConstructorImobiliarModel.NumeComplet;
                toEditUserProfile.PhoneNumber = editConstructorImobiliarModel.PhoneNumber;

                this.unitOfWork.UsersRepository.UpdateUserProfile(toEditUserProfile);

                //Get here user and update user profile and also agency = combination of both from agency
                var toEditConstructor = this.unitOfWork.ConstructoriRepository.Single(x => x.Id == editConstructorImobiliarModel.ConstructorId);

                //Check rights

                toEditConstructor.Nume = editConstructorImobiliarModel.ConstructorNume;
                toEditConstructor.Descriere = editConstructorImobiliarModel.ConstructorDescriere;
                toEditConstructor.Website = editConstructorImobiliarModel.ConstructorWebsite;
                toEditConstructor.Telefon = editConstructorImobiliarModel.ConstructorTelefon;

                this.unitOfWork.ConstructoriRepository.Update(toEditConstructor);

                log.Debug("Constructor imobiliar details edited for user " + User.Identity.Name);

                if (editConstructorImobiliarModel.ProfileImage != null)
                {
                    //this.unitOfWork.UsersRepository.AddImageForConstructor(editConstructorImobiliarModel.ProfileImage, toEditConstructor.Id);
                    log.Debug("Constructor image added for " + User.Identity.Name);
                }

                this.unitOfWork.Complete();

                TempData["Message"] = "Modificarile au fost salvate.";
                return RedirectToAction("EditUserDetails");
            }

            return View(editConstructorImobiliarModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RestrictionarePrimireEmail(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.Id = id;
                var user = this.unitOfWork.UsersRepository.SingleOrDefault(x => x.Id == id);
                ViewBag.Email = user.Email;
            }
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RestrictionarePrimireEmail(string userId, string email)
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Id == userId);
            user.RestrictionatPrimireEmail = true;

            log.Warn($"User {email} RestrictionarePrimireEmail successfully, userId {userId}");

            this.unitOfWork.Complete();

            TempData["Message"] = "Dezabonare cu succes! Nu veti mai primi emailuri din partea apartamente24.ro!";

            return RedirectToAction("RestrictionarePrimireEmail", new { id = "" });
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private bool HasPassword()
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = this.unitOfWork.UsersRepository.Single(x => x.Email == User.Identity.Name);
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}