using System.ComponentModel.DataAnnotations.Schema;

namespace Imobiliare.Entities
{
  using System.ComponentModel.DataAnnotations;

  public class Oras : Entity
  {
    public string Nume { get; set; }

    public string CoordinateGps { get; set; }

    public string CodPostal { get; set; }

    public bool ResedintaJudet { get; set; }

    //Indica daca localitatea se afiseaza pe pagina principala, sau apare doar la cautari
    public bool LocalitateMica { get; set; }

    public int JudetId { get; set; }

    [ForeignKey(nameof(JudetId))]
    public Judet Judet { get; set; }

    public string DescriereLocalitate { get; set; }
  }
}