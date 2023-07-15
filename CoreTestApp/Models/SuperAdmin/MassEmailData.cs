using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using Imobiliare.Entities;

  public class MassEmailData
  {
    public MassEmailData()
    {
      this.EmailTemplates = new List<EmailTemplate>();
    }
    public List<EmailTemplate> EmailTemplates { get; set; } 
  }
}