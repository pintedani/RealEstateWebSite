using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;


namespace Imobiliare.ServiceLayer.EmailService
{
    public class EmailManagerService : IEmailManagerService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailManagerService));

        private const string ADMIN_EMAIL_ADDRESS = "admin@apartamente24.ro";

        private IEmailSender emailManager;

        private readonly IUnitOfWork unitOfWork;

        public EmailManagerService(IEmailSender emailManager, IUnitOfWork unitOfWork)
        {
            this.emailManager = emailManager;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmailTemplate> GetAllEmailTemplates()
        {
            return this.unitOfWork.EmailTemplateRepository.GetAllEmailTemplates();
        }

        public EmailTemplate GetEmailTemplate(int id)
        {
            return this.unitOfWork.EmailTemplateRepository.GetEmailTemplate(id);
        }

        public EmailTemplate GetEmailTemplate(string humanReadableId)
        {
            return this.unitOfWork.EmailTemplateRepository.GetEmailTemplate(humanReadableId);
        }

        public void AddEmailTemplate(EmailTemplate emailTemplate)
        {
            this.unitOfWork.EmailTemplateRepository.AddEmailTemplate(emailTemplate);
            this.unitOfWork.Complete();
        }

        public void UpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            this.unitOfWork.EmailTemplateRepository.UpdateEmailTemplate(emailTemplate);
            this.unitOfWork.Complete();
        }

        public void DeleteEmailTemplate(int id)
        {
            this.unitOfWork.EmailTemplateRepository.DeleteEmailTemplate(id);
            this.unitOfWork.Complete();
        }

        public EmailSendStatus NotifyAdminContactForm(string phone, string email, string message)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.AdminContactRelated);

            return this.SendEmail(ADMIN_EMAIL_ADDRESS, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, message, phone, email));
        }

        public EmailSendStatus NotifyAdminRaportAbuzAnunt(int anuntId, string mesaj, string selectAbuzTip, string emailContact, string titluAnunt)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.NotifyAdminRaportAbuzAnunt);
            return this.SendEmail(ADMIN_EMAIL_ADDRESS, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, selectAbuzTip, titluAnunt, anuntId, mesaj, emailContact));
        }

        public EmailSendStatus NotifyAdminAnuntAdaugat(int anuntId, string titluAnunt, string userName)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.NotifyAdminAnuntAdaugat);
            return this.SendEmail(ADMIN_EMAIL_ADDRESS, string.Format(emailTemplate.Titlu, userName), string.Format(emailTemplate.Mesaj, userName, titluAnunt, anuntId));
        }

        public EmailSendStatus NotifyAdminAnunturiAdaugate(int intervalOre, int numarAnunturi)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.NotifyAdminAnunturiAdaugate);
            return this.SendEmail(ADMIN_EMAIL_ADDRESS, string.Format(emailTemplate.Titlu, intervalOre, numarAnunturi), string.Format(emailTemplate.Mesaj, intervalOre, numarAnunturi));
        }

        public EmailSendStatus AnuntRelatedEmail(string emailTemplateHumanReadableId, string titluAnunt, int anuntId, string userId, string emailrecipient, string mesajCustom)
        {
            var emailTemplate = this.unitOfWork.EmailTemplateRepository.GetEmailTemplate(emailTemplateHumanReadableId);

            return this.SendEmail(emailrecipient, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, titluAnunt, anuntId, userId));
        }

        public EmailSendStatus AnunturiExpirateRelatedEmail(string emailrecipient, int numarAnunturiExpirate)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.NotifyUserAnunturiExpirate);

            return this.SendEmail(emailrecipient, string.Format(emailTemplate.Titlu, numarAnunturiExpirate), string.Format(emailTemplate.Mesaj, numarAnunturiExpirate));
        }

        public EmailSendStatus UserRelatedEmail(string emailTemplateHumanReadableId, string userId, string emailrecipient)
        {
            var emailTemplate = this.unitOfWork.EmailTemplateRepository.GetEmailTemplate(emailTemplateHumanReadableId);

            return this.SendEmail(emailrecipient, emailTemplate.Titlu, emailTemplate.Mesaj);
        }

        public EmailSendStatus AnuntAprobatRelatedEmail(string titluAnunt, int anuntId, string userId, string emailrecipient)
        {
            //Exception for getting anunt aprobat message by Human readable Id
            var emailTemplate = this.unitOfWork.EmailTemplateRepository.GetEmailTemplate("Anunt aprobat");

            return this.SendEmail(emailrecipient, string.Format(emailTemplate.Titlu, titluAnunt), string.Format(emailTemplate.Mesaj, anuntId, titluAnunt));
        }

        public EmailSendStatus SendAgentImobiliarContactEmail(
          string emailAgent,
          string mesaj,
          string emailCumparator,
          string telefonCumparator)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.UserToAgentImobiliarContactRelated);
            this.SendEmail(
              emailAgent,
              emailTemplate.Titlu,
              string.Format(emailTemplate.Mesaj, mesaj, telefonCumparator, emailCumparator));

            if (this.unitOfWork.SystemSettingsRepository.SystemSettings.NotifyAdminByEmailWhenUserContactsAnotherUser)
            {
                //Send/Inform also to admin
                var emailTemplate2 =
                    this.unitOfWork.EmailTemplateRepository.GetAllEmailTemplates()
                    .First(x => x.EmailTemplateCategory == EmailTemplateCategory.UserToUserContactRelatedInformAdmin);
                this.SendEmail(
                  ADMIN_EMAIL_ADDRESS,
                  emailTemplate2.Titlu,
                  string.Format(emailTemplate2.Mesaj, 0, mesaj));
            }
            return EmailSendStatus.Success;
        }

        public void TrimitereRaportAdmin(List<UserProfile> adminWithRapoarteRecieveOption, string finalEmail)
        {
            foreach (var admin in adminWithRapoarteRecieveOption)
            {
                this.SendEmail(admin.Email, "Raport activitate site " + DateTime.Now.ToShortDateString(), finalEmail);
            }
        }

        public EmailSendStatus SendAnuntCereDetaliiEmail(string email, int id, string title)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.UserToUserContactRelated);
            this.SendEmail(email, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, id, title));

            if (this.unitOfWork.SystemSettingsRepository.SystemSettings.NotifyAdminByEmailWhenUserContactsAnotherUser)
            {
                //Send/Inform also to admin
                var emailTemplate2 = this.GetEmailTemplate(EmailTemplateCategory.UserToUserContactRelatedInformAdmin);
                this.SendEmail(ADMIN_EMAIL_ADDRESS, emailTemplate2.Titlu, string.Format(emailTemplate2.Mesaj, id, title));
            }
            return EmailSendStatus.Success;
        }

        public EmailSendStatus UserAccountConfirmationEmail(string emailUser, string userId, string confirmationToken)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.UserAccountConfirmation);
            return this.SendEmail(emailUser, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, userId, confirmationToken));
        }

        public EmailSendStatus UserPasswordResetEmail(string emailUser, string confirmationToken)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.UserAccountPasswordReset);
            return this.SendEmail(emailUser, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, confirmationToken));
        }

        public EmailSendStatus UserAccountConfirmedWelcomeMessageEmail(string emailUser)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.UserAccountConfirmedWelcomeMessage);
            return this.SendEmail(emailUser, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, emailUser));
        }

        public EmailSendStatus ExternalLoginWelcomeMessageEmail(string emailUser, string localPassword)
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.ExternalLoginWelcomeMessage);
            return this.SendEmail(emailUser, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, emailUser, localPassword));
        }

        public EmailSendStatus MassEmailSendEmail(string emailUser, string emailTemplateHumanReadableId)
        {
            var emailTemplate = this.unitOfWork.EmailTemplateRepository.GetEmailTemplate(emailTemplateHumanReadableId);
            return this.SendEmail(emailUser, emailTemplate.Titlu, string.Format(emailTemplate.Mesaj, emailUser));
        }

        public string EmailFooter()
        {
            var emailTemplate = this.GetEmailTemplate(EmailTemplateCategory.EmailFooter).Mesaj;
            return emailTemplate;
        }

        private EmailSendStatus SendEmail(string emailUser, string titlu, string mesaj)
        {
            //TODO DAPI use FakeEmailManager instance instead of checking flag
            if (!this.unitOfWork.SystemSettingsRepository.SystemSettings.UseFakeEmailManager)
            {
                var user = this.unitOfWork.UsersRepository.SingleOrDefault(x => x.Email == emailUser);

                if(user != null)
                {
                    if (!user.RestrictionatPrimireEmail)
                    {
                        return emailManager.SendUserEmail(emailUser, titlu, mesaj, EmailFooter(), user.Id);
                    }
                    else
                    {
                        var message = $"RestrictionatPrimireEmail Email manager did not sent email to User with email {emailUser} cu titlul {titlu} si mesajul {mesaj}";
                        log.Warn(message);

                        this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Warning, message);
                        this.unitOfWork.Complete();
                        return EmailSendStatus.RestrictionatPrimireEmail;
                    }
                }
                else if (emailUser == ConfigurationManager.AppSettings["EmailAdminFromAddress"].Trim())
                {
                    return emailManager.SendUserEmail(emailUser, titlu, mesaj, EmailFooter(), ConfigurationManager.AppSettings["EmailAdminFromAddress"]);
                }
                else
                {
                    var message = $"Email manager did not sent email to NOT EXISTING User in DB to email {emailUser} cu titlul {titlu} si mesajul {mesaj}";
                    log.Warn(message);
                    this.unitOfWork.AuditTrailRepository.AddAuditTrail(AuditTrailCategory.Warning, message);
                    this.unitOfWork.Complete();

                    return EmailSendStatus.UserNotExistsInDb;
                }
            }
            else
            {
                log.DebugFormat("FAKE email manager sent email  to {0} cu titlul {1} si mesajul {2}", emailUser, titlu, mesaj);
                return EmailSendStatus.Success;
            }
        }

        private EmailTemplate GetEmailTemplate(EmailTemplateCategory emailTemplateCategory)
        {
            return this.unitOfWork.EmailTemplateRepository.Find(x => x.EmailTemplateCategory == emailTemplateCategory).First();
        }
    }
}
