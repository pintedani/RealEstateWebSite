using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class RapoarteAnunturiDetalii
  {
    public RapoarteAnunturiDetalii()
    {
      AnunturiNoiAdaugate = new List<Imobil>();
      AnunturiReactualizate = new List<Imobil>();
      UseriCreati = new List<UserProfile>();
    }

    public List<Imobil> AnunturiNoiAdaugate { get; set; }

    public List<Imobil> AnunturiReactualizate { get; set; }

    public List<UserProfile> UseriCreati { get; set; }
  }
}