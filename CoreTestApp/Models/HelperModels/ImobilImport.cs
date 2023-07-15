namespace Imobiliare.UI.Models.HelperModels
{
  using Imobiliare.Entities;

  public class ImobilImport
  {
    public string Title { get; set; }
    public string Descriere { get; set; }

    public int Price { get; set; }
    public int Suprafata { get; set; }
    public string Poze { get; set; }

    public int JudetId { get; set; }
    public int OrasId { get; set; }
    public int CartierId { get; set; }

    public string Strada { get; set; }

    //public TipOferta TipOferta { get; set; }

    public int UserId { get; set; }
    public int Valabilitate { get; set; }

    public int Etaj { get; set; }
    public int NumarTotalEtaje { get; set; }

    public int VechimeImobil { get; set; }
    public int NrBalcoane { get; set; }
    public int NrBai { get; set; }
    public string ContactTelefon { get; set; }

    public string ContactEmail { get; set; }
    public int NumarCamere { get; set; }

    public string LinkExtern { get; set; }

    public string GoogleMarkerCoordinates { get; set; }

    public TipVanzator TipVanzator { get; set; }

    public bool Garaj { get; set; }
    public bool CT { get; set; }
    public bool AerConditionat { get; set; }
    public bool LocInPivnita { get; set; }
    public bool Negociabil { get; set; }
  }
}