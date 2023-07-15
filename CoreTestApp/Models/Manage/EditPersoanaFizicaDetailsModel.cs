using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using System.ComponentModel.DataAnnotations;

  using Imobiliare.Entities;

  public class EditPersoanaFizicaDetailsModel : IValidatableObject
  {
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string NumeComplet { get; set; }

    public string PhoneNumber { get; set; }

    public bool AbonatLaNewsLetter { get; set; }

    public List<Imobil> Anunturi { get; set; }

    public bool TrustedUser { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
    public DateTime? UserCreateDate { get; set; }

    public TipVanzator TipVanzator { get; set; }

    //This properties are for when user changes from PF to Agentie/Constructor

    public string NumeAgentieImobiliara { get; set; }

    public HttpPostedFileBase ProfileImage { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      List<ValidationResult> validationResults = new List<ValidationResult>();
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
}