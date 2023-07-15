using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
  public class StireViewModel
  {
    public Stire Stire { get; set; }

    public Stire StireAnterioara { get; set; }

    public Stire StireUrmatoare { get; set; }

    public List<Imobil> LastAddedImobils { get; set; }
  }
}