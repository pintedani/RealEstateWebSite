using System.Collections.Generic;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class ImobilCreateData
  {
    public ImobilCreateData()
    {
      ZoomLevel = 12;
    }

    public Imobil ImobilToEdit { get; set; }

    public List<Judet> Judete { get; set; }

    public TipOfertaGen TipOfertaGen { get; set; }

    public TipProprietate TipProprietate { get; set; }

    public string GpsCoordinates { get; set; }

    public int ZoomLevel { get; set; }

    public UserProfile UserProfile { get; set; }

    public List<Cartier> Cartiere { get; set; } 
  }
}