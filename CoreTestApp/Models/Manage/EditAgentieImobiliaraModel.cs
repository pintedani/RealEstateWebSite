using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using System.ComponentModel.DataAnnotations.Schema;

  using Imobiliare.Entities;

  public class EditAgentieImobiliaraModel : IValidatableObject
  {
    public Agentie Agentie { get; set; }

    public IFormFile ProfileImage { get; set; }

    public int OrasSelect { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      var validationResults = new List<ValidationResult>();
      if (string.IsNullOrEmpty(Agentie.Nume))
      {
        validationResults.Add(new ValidationResult("Va rugam specificati numele agentiei imobiliara pentru a putea modifica tipul vanzatorului", new List<string>() { nameof(Agentie.Nume) }));
      }
      return validationResults;
    }
  }
}