namespace CoreTestApp.Models
{
  using System;
  using System.ComponentModel;

  public class EmailTemplate : Entity
  {
    public string Titlu { get; set; }

    public string Mesaj { get; set; }

    [DisplayName("Created")]
    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    [DisplayName("Category")]
    public EmailTemplateCategory EmailTemplateCategory { get; set; }

    [DisplayName("NiceId")]
    ///for differentiating between templates with same category
    /// to display on the ui where user can also select
    public string HumanReadableEmailIdentifier { get; set; }
  }

  public enum EmailTemplateCategory
  {
    /// <summary>
    /// Admin trimitere mesaje legate de anunturi
    /// Numar mesaje nelimitat
    /// </summary>
    AdminAnuntRelated = 0,

    /// <summary>
    /// Admin trimite mesaje la useri direct
    /// Numar mesaje nelimitat
    /// </summary>
    AdminUserRelated = 1,

    /// <summary>
    /// Formular contact
    /// Numar mesaje 1 max
    /// </summary>
    AdminContactRelated = 2,

    /// <summary>
    /// Admin primeste msg ca s-a adaugat anbunt
    /// Numar mesaje 1 max
    /// </summary>
    NotifyAdminAnuntAdaugat = 3,

    ///// <summary>
    ///// Admin primeste msg ca s-a react anunt prin email
    ///// Numar mesaje 1 max
    ///// </summary>
    //NotifyAdminAnuntReactualizatPrinEmail = 4,

    ///// <summary>
    ///// Admin primeste msg ca s-a dezact anunt prin email
    ///// Numar mesaje 1 max
    ///// </summary>
    //NotifyAdminAnuntDezactivatPrinEmail = 5,

    /// <summary>
    /// Admin primeste msg de abuz anunt
    /// Numar mesaje 1 max
    /// </summary>
    NotifyAdminRaportAbuzAnunt = 6,

    /// <summary>
    /// User cere detalii la alt user prin formular contact
    /// Numar mesaje 1 max
    /// </summary>
    UserToUserContactRelated = 7,

    /// <summary>
    /// Informeaza admin ca un user a contactat pe alt user prin anunt contact form
    /// </summary>
    UserToUserContactRelatedInformAdmin = 8,

    /// <summary>
    /// Email de confirmare cont
    /// Numar mesaje 1 max
    /// </summary>
    UserAccountConfirmation = 9,

    /// <summary>
    /// Email de resetare parola
    /// Numar mesaje 1 max
    /// </summary>
    UserAccountPasswordReset = 10,

    /// <summary>
    /// Email sent to user after he confirms the email on the website
    /// </summary>
    UserAccountConfirmedWelcomeMessage = 11,

    /// <summary>
    /// Email sent to user after connecting first with external login
    /// </summary>
    ExternalLoginWelcomeMessage = 12,

    /// <summary>
    /// Generic email footer
    /// </summary>
    EmailFooter = 13,

    /// <summary>
    /// Mass email category
    /// </summary>
    MassEmail = 14,

    NotifyAdminAnunturiAdaugate = 15,

    NotifyUserAnunturiExpirate = 16,

    UserToAgentImobiliarContactRelated = 17
  }
}
