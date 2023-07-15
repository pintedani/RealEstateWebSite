using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class MesajConversationModel
  {
    public List<Mesaj> Mesaje { get; set; }

    public Mesaj MesajSelectat { get; set; }

    //public Imobil Imobil { get; set; }

    public string LoggedInUserEmail { get; set; }

    public string ToUserEmail { get; set; }
  }
}