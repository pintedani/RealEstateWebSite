using System.Linq;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;

namespace Imobiliare.Repositories
{
    public class SystemSettingsRepository : Repository<SystemSettings>, ISystemSettingsRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SystemSettingsRepository));

        public SystemSettingsRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Entities.SystemSettings.Id)))
        {
        }

        public SystemSettings SystemSettings
        {
            get
            {
                var systemSettings = this.DbContext.SystemSettings.FirstOrDefault();
                if (systemSettings == null)
                {
                    var defaultSystemSettings = new SystemSettings();
                    this.DbContext.SystemSettings.Add(defaultSystemSettings);

                    //Need to save explicit because this is not called from any unitofwork
                    this.DbContext.SaveChanges();
                    return defaultSystemSettings;
                }
                return systemSettings;
            }
        }

        public void UpdateSystemSettings(SystemSettings systemSettings)
        {
            var systemSettingsOld = this.DbContext.SystemSettings.First();
            systemSettingsOld.AutoApproveAnunturi = systemSettings.AutoApproveAnunturi;
            systemSettingsOld.CapchaEnabled = systemSettings.CapchaEnabled;
            systemSettingsOld.DefaultPageSizeAdminPageAnunturi = systemSettings.DefaultPageSizeAdminPageAnunturi;
            systemSettingsOld.DefaultPageSizeAdminPageUsers = systemSettings.DefaultPageSizeAdminPageUsers;
            systemSettingsOld.DefaultPageSizeMainPages = systemSettings.DefaultPageSizeMainPages;
            systemSettingsOld.UseFakeEmailManager = systemSettings.UseFakeEmailManager;
            systemSettingsOld.NotifyAdminByEmailWhenUserChangedAnuntByEmail = systemSettings.NotifyAdminByEmailWhenUserChangedAnuntByEmail;
            systemSettingsOld.NotifyAdminByEmailWhenUserContactsAnotherUser = systemSettings.NotifyAdminByEmailWhenUserContactsAnotherUser;
            systemSettingsOld.AutoSendAnuntExpiratEmails = systemSettings.AutoSendAnuntExpiratEmails;
            systemSettingsOld.LogsRetrieveNumber = systemSettings.LogsRetrieveNumber;
        }
    }
}
