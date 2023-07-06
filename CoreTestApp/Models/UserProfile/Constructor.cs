using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreTestApp.Models
{
  public class Constructor : Entity
  {
    public string Nume { get; set; }

    [ForeignKey(nameof(OrasId))]
    public Oras Oras { get; set; }

    public int? OrasId { get; set; }

    public List<UserProfile> ConstructorUserProfiles { get; set; }

    public string Descriere { get; set; }

    public string Website { get; set; }

    public string Poza { get; set; }

    public string Telefon { get; set; }
  }
}
