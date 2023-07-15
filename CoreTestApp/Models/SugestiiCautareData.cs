using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  public class SugestiiCautareData
  {
    public SugestiiCautareData(string text, List<string> rezultate)
    {
      this.Text = text;
      this.Rezultate = rezultate;
    }

    public string Text { get; set; }

    public List<string> Rezultate { get; set; }
  }
}