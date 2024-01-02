using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Logging;

namespace Imobiliare.UI.ScheduledTasks
{
    public class AnunturiExpirateCheckerJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnunturiExpirateCheckerJob));

        private IUnitOfWork unitOfWork;

        private IEmailManagerService emailManagerService;

        public AnunturiExpirateCheckerJob(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService)
        {
            this.unitOfWork = unitOfWork;
            this.emailManagerService = emailManagerService;
        }

        public void Execute()
        {
            try
            {
                var expiredAnunturi = this.unitOfWork.AnunturiRepository.CheckForExpiredAnunturi();
                if (expiredAnunturi.Any())
                {
                    this.unitOfWork.Complete();

                    var groupedExpiredAnunturi = new Dictionary<UserProfile, List<Imobil>>();
                    foreach (var anunt in expiredAnunturi)
                    {
                        if (groupedExpiredAnunturi.Keys.Contains(anunt.UserProfile))
                        {
                            groupedExpiredAnunturi[anunt.UserProfile].Add(anunt);
                        }
                        else
                        {
                            groupedExpiredAnunturi.Add(anunt.UserProfile, new List<Imobil>() { anunt });
                        }
                    }

                    if (this.unitOfWork.SystemSettingsRepository.SystemSettings.AutoSendAnuntExpiratEmails)
                    {
                        foreach (var groupedExpiredAnunt in groupedExpiredAnunturi.Where(x => x.Key.Role != Role.Administrator))
                        {
                            if (groupedExpiredAnunt.Value.Count == 1)
                            {
                                //Good for PF - to give possibility to approve directly from email
                                var anunt = groupedExpiredAnunt.Value.Single();
                                this.emailManagerService.AnuntRelatedEmail(
                                  "Anunt expirat",
                                  "Anunt expirat pe apartamente24.ro",
                                  anunt.Id,
                                  anunt.UserProfile.Id,
                                  anunt.UserProfile.Email,
                                  "");
                            }
                            else
                            {
                                //Sent for multiple expired anunt/define new email
                                this.emailManagerService.AnunturiExpirateRelatedEmail(groupedExpiredAnunt.Key.Email, groupedExpiredAnunt.Value.Count);
                            }
                        }
                    }
                    else
                    {
                        log.Warn("Auto send anunturi expirat emails disabled!");
                    }

                    if (expiredAnunturi.Count == 0)
                    {
                        log.Debug("[SERVICE EXPIRED ANUNTURI] = No expired anunturi....");
                    }
                    else
                    {
                        log.Debug($"[SERVICE EXPIRED ANUNTURI] = {expiredAnunturi.Count.ToString()} anunturi expirate -------------------- ");
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat($"[SERVICE EXPIRED ANUNTURI] = Error while checking for expired anuturi {ex.Message}");
                //throw;
            }
        }
    }
}