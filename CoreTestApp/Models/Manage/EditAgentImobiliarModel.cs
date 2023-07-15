using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
  using System.ComponentModel.DataAnnotations;

  using Imobiliare.Entities;

  public class EditAgentImobiliarModel
  {
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string NumeComplet { get; set; }

    public string PhoneNumber { get; set; }

    public bool AbonatLaNewsLetter { get; set; }

    public List<Imobil> Anunturi { get; set; }

    public bool TrustedUser { get; set; }

    public Agentie Agentie { get; set; }

    public string Picture { get; set; }

    public string NumeAgentieImobiliara { get; set; }

    public bool IsAgentieAdmin { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
    public DateTime? UserCreateDate { get; set; }

    public HttpPostedFileBase ProfileImage { get; set; }
  }
}