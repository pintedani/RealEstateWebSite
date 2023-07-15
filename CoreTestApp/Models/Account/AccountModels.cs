namespace Imobiliare.UI.Models
{
  using System.Web;
  using System.ComponentModel.DataAnnotations;
  using System.Data.Entity;
  using Imobiliare.Entities;

  //public class UsersContext : DbContext
  //{
  //  public UsersContext()
  //    : base("DefaultConnection")
  //  {
  //  }

  //  public DbSet<UserProfile> UserProfiles { get; set; }
  //}

  public class RegisterExternalLoginModel
  {
    [Required]
    [Display(Name = "User name")]
    public string UserName { get; set; }

    public string ExternalLoginData { get; set; }
  }

  public class LocalPasswordModel
  {
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Parola curentă")]
    public string OldPassword { get; set; }

    [Required]
    //TODO Change password length
    [StringLength(100, ErrorMessage = "{0} trebuie sa contina cel putin {2} charactere.", MinimumLength = 1)]
    [DataType(DataType.Password)]
    [Display(Name = "Parola nouă")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmă parola nouă")]
    [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Parola noua si confirmarea parolei nu coincid.")]
    public string ConfirmPassword { get; set; }
  }

  public class LoginModel
  {
    [Required]
    [Display(Name = "Email")]
    //TODO add back atribute[Email]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string Password { get; set; }

    [Display(Name = "Tine-mă minte?")]
    public bool RememberMe { get; set; }
  }

  public class RegisterModel
  {
    [Required(ErrorMessage = "Completați adresa de email")]
    [Display(Name = "Email")]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalid!")]
    [StringLength(40, ErrorMessage = "Lungime email nu trebuie să depășească 40 de caractere")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Introduceți o parolă personală")]
    //TODO Change password length
    [StringLength(100, ErrorMessage = "{0} trebuie sa conțină cel putin {2} charactere.", MinimumLength = 1)]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmă parola")]
    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Parola noua si confirmarea parolei nu coincid.")]
    public string ConfirmPassword { get; set; }

    public Role Role { get; set; }
    public string Email { get; set; }
    //public bool PersoanaFizica { get; set; }

    public TipVanzator TipVanzator { get; set; }
    public string Picture { get; set; }
    public string Telefon { get; set; }

    [StringLength(40, ErrorMessage = "Numele agenției nu trebuie să depașească 40 de caractere")]
    public string NumeAgentieImobiliara { get; set; }

    public HttpPostedFileBase ProfileImage { get; set; }
  }

  public class ExternalLogin
  {
    public string Provider { get; set; }
    public string ProviderDisplayName { get; set; }
    public string ProviderUserId { get; set; }
  }
}