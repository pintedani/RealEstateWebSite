namespace Imobiliare.ServiceLayer.Interfaces
{
  using System.Collections.Generic;

  using Entities;
    using Imobiliare.Entities;

    public interface IEmailManagerService
  {
    #region Email administration methods

    IEnumerable<EmailTemplate> GetAllEmailTemplates();

    EmailTemplate GetEmailTemplate(int id);

    //to diff between emails which are in the same category but admin choses from one of those on the UI
    EmailTemplate GetEmailTemplate(string humanReadableId);

    void AddEmailTemplate(EmailTemplate emailTemplate);

    void UpdateEmailTemplate(EmailTemplate emailTemplate);

    void DeleteEmailTemplate(int id);

    #endregion

    #region To Admin notification Emails

    EmailSendStatus NotifyAdminContactForm(string phone, string email, string message);

    EmailSendStatus NotifyAdminRaportAbuzAnunt(int anuntId, string mesaj, string selectAbuzTip, string emailContact, string titluAnunt);

    EmailSendStatus NotifyAdminAnuntAdaugat(int anuntId, string titluAnunt, string userName);

    EmailSendStatus NotifyAdminAnunturiAdaugate(int intervalOre, int numarAnunturi);

    //EmailSendStatus NotifyAdminAnuntReactualizat(int anuntId);

    //EmailSendStatus NotifyAdminAnuntDezactivat(int anuntId);

    #endregion

    #region From Admin to Anunt send email messages

    EmailSendStatus AnuntRelatedEmail(string emailTemplateHumanReadableId, string titluAnunt, int anuntId, string userId, string emailrecipient, string mesajCustom);

    EmailSendStatus AnunturiExpirateRelatedEmail(string emailrecipient, int numarAnunturiExpirate);

    EmailSendStatus AnuntAprobatRelatedEmail(string titluAnunt, int anuntId, string userId, string emailrecipient);

    #endregion

    #region From Admin to User send email Messages

    EmailSendStatus UserRelatedEmail(string emailTemplateHumanReadableId, string userId, string emailrecipient);

    #endregion

    #region User to User contact email Messages

    EmailSendStatus SendAnuntCereDetaliiEmail(string email, int id, string title);

    #endregion

    #region Account confirmation email messages

    EmailSendStatus UserAccountConfirmationEmail(string emailUser, string userId, string confirmationToken);

    EmailSendStatus UserPasswordResetEmail(string emailUser, string confirmationToken);

    EmailSendStatus UserAccountConfirmedWelcomeMessageEmail(string emailUser);

    EmailSendStatus ExternalLoginWelcomeMessageEmail(string emailUser, string localPassword);

    #endregion

    #region Mass Email send email messages

    EmailSendStatus MassEmailSendEmail(string emailUser, string emailTemplateHumanReadableId);

    #endregion

    EmailSendStatus SendAgentImobiliarContactEmail(string emailAgent, string mesaj, string emailCumparator, string telefonCumparator);

    void TrimitereRaportAdmin(List<UserProfile> adminWithRapoarteRecieveOption, string finalEmail);
  }
}
