using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemSettingsController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SystemSettingsController));

        private IUnitOfWork unitOfWork;

        public SystemSettingsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: SystemSettings
        public ActionResult Index()
        {
            var systemSettings = this.unitOfWork.SystemSettingsRepository.SystemSettings;
            return View(systemSettings);
        }

        public ActionResult Editeaza()
        {
            var systemSettings = this.unitOfWork.SystemSettingsRepository.SystemSettings;
            return View(systemSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editeaza(
          [Bind("Id,DefaultPageSizeAdminPageAnunturi,DefaultPageSizeAdminPageUsers,DefaultPageSizeMainPages,AutoApproveAnunturi,CapchaEnabled,UseFakeEmailManager,NotifyAdminByEmailWhenUserContactsAnotherUser,NotifyAdminByEmailWhenUserChangedAnuntByEmail,AutoSendAnuntExpiratEmails,LogsRetrieveNumber"
          )] SystemSettings systemSettings)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.SystemSettingsRepository.UpdateSystemSettings(systemSettings);
                this.unitOfWork.Complete();
                TempData["Message"] = "Setari updatate cu succes";
                log.DebugFormat("SystemSettings updated by {0}", User.Identity.Name);
                return RedirectToAction("Index");
            }
            return View(systemSettings);
        }
    }
}
