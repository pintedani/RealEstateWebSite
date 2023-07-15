using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class AdaugareOptiuniData
  {
    public Judet SeletedJudet { get; set; }

    public Oras SeletedOras { get; set; }

    public Cartier SeletedCartier { get; set; }

    public ImobilFilter ImobilFilter { get; set; }

    public List<Judet> Judets { get; set; }

    //Based on whole Filter
    public Dictionary<Oras, int> SelectableOrasesWithNumber { get; set; }

    public Dictionary<Cartier, int> SelectableCartiersWithNumber { get; set; }
  }
}