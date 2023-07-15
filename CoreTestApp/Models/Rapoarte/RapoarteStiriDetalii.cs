using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class RapoarteStiriDetalii
  {
    public RapoarteStiriDetalii()
    {
      Stiri = new List<Stire>();
    }

    public List<Stire> Stiri { get; set; }
  }
}