using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalid!")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        //public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Completați adresa de email")]
        [Display(Name = "Email")]
        //TODO add back atribute[Email]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Completați parola")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Display(Name = "Pastrează-mă logat")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : IValidatableObject
  {
        [Required(ErrorMessage = "Completați adresa de email")]
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email invalid!")]
        [StringLength(40, ErrorMessage = "Lungime email nu trebuie sa depășească 40 de caractere")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Introduceți o parolă personală")]
        //TODO Change password length
        [StringLength(100, ErrorMessage = "{0} trebuie sa contină cel putin {2} charactere.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă parolă")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Parola nouă si confirmarea parolei nu coincid.")]
        public string ConfirmPassword { get; set; }

        public Role Role { get; set; }
        public string Email { get; set; }
        //public bool PersoanaFizica { get; set; }

        public TipVanzator TipVanzator { get; set; }
        public string Picture { get; set; }
        public string Telefon { get; set; }

        [StringLength(40, ErrorMessage = "Numele agenției nu trebuie sa depăseasca 40 de caractere")]
        public string NumeAgentieImobiliara { get; set; }

        public IFormFile ProfileImage { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var validationResults = new List<ValidationResult>();
      if (TipVanzator == TipVanzator.AgentieImobiliara)
      {
        if (string.IsNullOrEmpty(NumeAgentieImobiliara))
        {
          validationResults.Add(new ValidationResult("Va rugam specificati numele agentiei imobiliara pentru a putea modifica tipul vanzatorului", new List<string>() { nameof(NumeAgentieImobiliara) }));
        }
      }
      return validationResults;
    }
  }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Emailul Dumneavoastra")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} trebuie sa fie cel putin {2} caracter lungime.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola noua")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmati parola noua")]
        [Compare("Password", ErrorMessage = "Parola si confirmarea parolei nu coincid.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
