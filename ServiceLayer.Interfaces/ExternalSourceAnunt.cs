namespace Imobiliare.ServiceLayer.Interfaces
{
  public class ExternalSourceAnunt
  {
    public string Titlu { get; set; }
    
    public string Descriere { get; set; }

    public string Oras { get; set; }

    public string TipOferta { get; set; }

    public string TipVanzator { get; set; }

    public int Suprafata { get; set; }

    public int Price { get; set; }

    public int NumarCamere { get; set; }

    public int Etaj { get; set; }

    public bool Decomandat { get; set; }

    public string Telefon { get; set; }

    public string Adresa { get; set; }

    public bool LocParcare { get; set; }

    public string LinkExtern { get; set; }

    public string EroareParsare { get; set; }
  }
}