using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;

namespace Imobiliare.UI.ScheduledTasks.Jobs
{
    using Logging;
    using System.Diagnostics;

    public class TrimitereRapoarteAdminJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnunturiAdaugateCheckerJob));

        private IUnitOfWork unitOfWork;

        private IEmailManagerService emailManagerService;

        public TrimitereRapoarteAdminJob(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService)
        {
            this.unitOfWork = unitOfWork;
            this.emailManagerService = emailManagerService;
        }
        public void Execute()
        {
            var lastRaport = this.unitOfWork.RaportActivitateRepository.GetLastRaportActivitate();
            //47 - 2 days without an hour, to be less that last scheduled time with 1 hour
            if(lastRaport == null || (DateTime.Now - lastRaport.CreateDateTime) > TimeSpan.FromHours(47))
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();

                    stopwatch.Start();

                    RaportActivitate raportActivitate = new RaportActivitate
                    {
                        CreateDateTime = DateTime.Now
                    };

                    log.Debug($"[SERVICE TRIMITE RAPOARTE ADMIN] Incepere compunere mesaj {DateTime.Now.ToString()} ");

                    string finalEmail = "Acest email se trimite o data la 48h, sau la orice pornire/repornire a serverului.<br/>" +
                                        "Mail trimis la urmatorii administratori(pentru abonare/dezabonare folositi pagina de admin)<br/>";

                    var adminWithRapoarteRecieveOption = this.unitOfWork.UsersRepository.Find(x => x.Role == Role.Administrator && x.ReceiveAdminReports);
                    raportActivitate.ReceiversCount = adminWithRapoarteRecieveOption.Count();
                    foreach (var admin in adminWithRapoarteRecieveOption)
                    {
                        finalEmail += admin.Email + " | ";
                    }

                    finalEmail += "<br/>"
                      + "-------------------------------"
                      + "<br/>";

                    var hours = 48;
                    var referenceDate = DateTime.Now.AddHours(-hours);
                    raportActivitate.LastConsideredHoursCount = hours;

                    var stiriAdaugate = this.unitOfWork.StiriRepository.Find(x => x.DateCreated > referenceDate);
                    raportActivitate.StiriAdaugateCount = stiriAdaugate.Count();
                    if (stiriAdaugate.Count() > 0)
                    {
                        finalEmail += "Stiri adaugate: <strong>" + stiriAdaugate.Count() +
                                      "</strong>"
                                      + "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    var ansambluriAdaugate = this.unitOfWork.AnsambluriRepository.Find(x => x.DateCreated > referenceDate);
                    raportActivitate.AnsambluriAdaugateCount = ansambluriAdaugate.Count();
                    if (ansambluriAdaugate.Count() > 0)
                    {
                        finalEmail += "Ansambluri adaugate: <strong>" + ansambluriAdaugate.Count() +
                                      "</strong>" +
                                      "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    var useriCreati = this.unitOfWork.UsersRepository.Find(x => x.UserCreateDate > referenceDate);
                    raportActivitate.UseriCreatiCount = useriCreati.Count();
                    if (useriCreati.Count() > 0)
                    {
                        finalEmail += "Useri creati: <strong>" + useriCreati.Count() +
                                        "</strong>"
                                        + "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    var anunturiAdaugate = this.unitOfWork.AnunturiRepository.Find(x => x.DataAdaugareInitiala > referenceDate);

                    var anunturiByUsers = anunturiAdaugate.Where(x => x.UserProfile.Role == Role.NormalUser);
                    raportActivitate.AnunturiByUsersCount = anunturiByUsers.Count();
                    if (anunturiByUsers.Count() > 0)
                    {
                        var unici = anunturiByUsers.Select(s => s.UserProfile.Email).Distinct();
                        finalEmail += "Numar anunturi adaugate de useri normali: <strong>" + anunturiByUsers.Count() + "</strong> Anunturi de catre <strong>" + unici.Count() + "</strong> useri <br/><br/>";
                        int i = 0;
                        foreach (var anunturiByUser in anunturiByUsers)
                        {
                            i++;
                            finalEmail += i + ". user: " + anunturiByUser.UserProfile.Email + ", Anunt: "
                                          + anunturiByUser.Title + ", oras " + anunturiByUser.Oras.Nume + "<br/>";

                        }

                        finalEmail += "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    var anunturiByAdmin = anunturiAdaugate.Where(x => x.UserProfile.Role == Role.Administrator);
                    raportActivitate.AnunturiByAdminCount = anunturiByAdmin.Count();
                    if (anunturiByAdmin.Count() > 0)
                    {
                        finalEmail += "Anunturi adaugate de admin: <strong>" + anunturiByAdmin.Count() + "</strong>";
                        finalEmail += "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";

                        int i = 0;
                        foreach (var anunt in anunturiByAdmin)
                        {
                            i++;
                            finalEmail += i + ". user: " + anunt.UserProfile.Email + ", Anunt: "
                                          + anunt.Title + ", oras " + anunt.Oras.Nume + "<br/>";
                        }
                        finalEmail += "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    var sessionStartedCount = this.unitOfWork.LogsRepository.Find(x => x.Date > referenceDate && x.Message.Contains("Session started for user"));
                    raportActivitate.SessionStartedCount = sessionStartedCount.Count();
                    if (sessionStartedCount.Count > 0)
                    {
                        var distinct =
                          sessionStartedCount
                            .GroupBy(car => car.Message)
                            .Select(g => g.First())
                            .ToList();
                        raportActivitate.SessionStartedDistinctCount = distinct.Count();
                        finalEmail += "Useri unici(acelasi ip): <strong>" + distinct.Count + "</strong>, in total <strong>" + sessionStartedCount.Count + "</strong> sesiuni pornite.";
                        finalEmail += "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    var auditTrails = this.unitOfWork.AuditTrailRepository.Find(x => x.Date > referenceDate && x.NotifyAdmin);
                    raportActivitate.AuditTrailsCount = auditTrails.Count();
                    if (auditTrails.Count > 0)
                    {
                        int i = 0;
                        foreach (var auditTrail in auditTrails)
                        {
                            i++;
                            finalEmail += i + ". Audit trail: " + auditTrail.Category + ", Mesaj: "
                                          + auditTrail.Message + ", user: " + auditTrail.UserName + "<br/>";
                        }
                        finalEmail += "<br/>"
                                      + "-------------------------------"
                                      + "<br/>";
                    }

                    raportActivitate.FinalEmail = finalEmail;
                    stopwatch.Stop();
                    raportActivitate.GenerareRaportTime = string.Format("{0:D2} hrs, {1:D2} mins, {2:D2} secs", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
                    raportActivitate.IsFakeEmailManager = this.unitOfWork.SystemSettingsRepository.Find(x => x.Id != 0)[0].UseFakeEmailManager;

                    this.unitOfWork.RaportActivitateRepository.Add(raportActivitate);
                    this.unitOfWork.Complete();
                    this.emailManagerService.TrimitereRaportAdmin(adminWithRapoarteRecieveOption, finalEmail);

                    log.Debug($"[SERVICE TRIMITE RAPOARTE ADMIN] S-a trimis email {0}, completat la {1} ", finalEmail, DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    log.Error($"[SERVICE TRIMITE RAPOARTE ADMIN] = Error while generating raport for admin: {ex.Message}");
                    //throw;
                }
            }
            else
            {
                log.Debug($"[SERVICE TRIMITE RAPOARTE ADMIN] Timespan of 48h not passed since last report was created. Skipping....");
            }
        }
    }
}