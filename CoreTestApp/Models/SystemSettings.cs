namespace CoreTestApp.Models
{
  public class SystemSettings : Entity
  {
    public SystemSettings()
    {
      DefaultPageSizeAdminPageAnunturi = 20;
      DefaultPageSizeAdminPageUsers = 30;
      DefaultPageSizeMainPages = 10;
      AutoApproveAnunturi = false;
      CapchaEnabled = true;
      UseFakeEmailManager = false;
      NotifyAdminByEmailWhenUserContactsAnotherUser = true;
      NotifyAdminByEmailWhenUserChangedAnuntByEmail = true;
      AutoSendAnuntExpiratEmails = false;
      LogsRetrieveNumber = 2000;
    }

    public int DefaultPageSizeAdminPageAnunturi { get; set; }

    public int DefaultPageSizeAdminPageUsers { get; set; }

    public int DefaultPageSizeMainPages { get; set; }

    public int LogsRetrieveNumber { get; set; }

    public bool AutoApproveAnunturi { get; set; }

    public bool CapchaEnabled { get; set; }

    public bool UseFakeEmailManager { get; set; }

    public bool NotifyAdminByEmailWhenUserContactsAnotherUser { get; set; }

    public bool NotifyAdminByEmailWhenUserChangedAnuntByEmail { get; set; }

    public bool AutoSendAnuntExpiratEmails { get; set; }

    public int MaxDbSize { get; set; }
  }
}
