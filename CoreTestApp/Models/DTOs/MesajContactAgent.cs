using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models.DTOs
{
  public class MesajContactAgent
  {
    public string Recaptcha { get; set; }
    public string EmailCumparator { get; set; }
    public string EmailAgent { get; set; }

    public string TelefonCumparator { get; set; }
    public string Mesaj { get; set; }
  }
}