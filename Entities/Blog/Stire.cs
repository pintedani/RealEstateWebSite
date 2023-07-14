using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imobiliare.Entities
{
  public class Stire : Entity
  {
    public Stire()
    {
      OrasSelect = 0;
    }

    public CategorieStire CategorieStire { get; set; }

    [Required]
    public string Titlu { get; set; }

    [Required]
    public string Continut { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public UserProfile UserProfile { get; set; }

    [ForeignKey("UserProfile")]
    public string UserId { get; set; }

    public int NumarVizualizari { get; set; }

    public string Poze { get; set; }

    public bool Active { get; set; }

    public bool AfiseazaPrimaPagina { get; set; }

    public string Keywords { get; set; }

    [DisplayName("Localitatea pentru afisare anunturi")]
    public int OrasSelect { get; set; }

    public string LinkExtern { get; set; }

    public string TitluUrl { get; set; }

    public string MetaDescription { get; set; }
  }

  public enum CategorieStire
  {
    Constructii,
    AmenajariInterioare,
    Credite
  }
}
