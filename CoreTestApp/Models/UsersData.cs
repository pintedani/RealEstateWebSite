namespace Imobiliare.UI.Models
{
  using System.Collections.Generic;
  using Imobiliare.Entities;


  public class UsersData
  {
    public List<UserProfile> UserProfiles { get; set; }

    public TipVanzator SelectedTipVanzator { get; set; }

    public Role SelectedRole { get; set; }

    public int NumberOfPages { get; set; }

    public int Page { get; set; }

    public List<EmailTemplate> EmailTemplates { get; set; }
  }
}