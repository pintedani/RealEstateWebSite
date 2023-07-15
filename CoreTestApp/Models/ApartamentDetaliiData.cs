using System.Collections.Generic;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class ApartamentDetaliiData
  {
    public Imobil Imobil { get; set; }
    
    //User profile which added Anunt
    public UserProfile UserProfile { get; set; }

    public List<Imobil> AnunturiSimilare { get; set; } 

    public bool IsCurrentUserAdmin { get; set; }
        public string LoggedInUserId { get; internal set; }
    }
}