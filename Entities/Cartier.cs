using System.ComponentModel.DataAnnotations.Schema;

namespace Imobiliare.Entities
{
  public class Cartier : Entity
  {
    public string? Nume { get; set; }

    public int OrasId { get; set; }

    [ForeignKey(nameof(OrasId))]
    public Oras Oras { get; set; }
  }
}