using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class RapoarteData
  {
    public RapoarteData()
    {
      Administratori = new List<UserProfile>();
      AnunturiNoiAdaugate = new List<Imobil>();
      AnunturiReactualizate = new List<Imobil>();
      Stiri = new List<Stire>();
      UseriCreati = new List<UserProfile>();
    }

    public List<UserProfile> Administratori { get; set; } 

    public List<Imobil> AnunturiNoiAdaugate { get; set; }

    public List<Imobil> AnunturiReactualizate { get; set; }

    public List<Stire> Stiri { get; set; } 

    public List<UserProfile> UseriCreati { get; set; } 
  }
}