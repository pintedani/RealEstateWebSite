using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestApp.Models
{
  public class Agentie : Entity
  {
    public Agentie()
    {
      this.AfisarePrimaPagina = true;
    }

    public string Nume { get; set; }

    [ForeignKey(nameof(OrasSelect))]
    public Oras Oras { get; set; }

    public int? OrasSelect { get; set; }

    public List<UserProfile> AgentieImobiliaraUserProfiles { get; set; }

    public string DescriereAgentie { get; set; }

    public string Website { get; set; }

    public string PozaAgentie { get; set; }

    public string TelefonAgentie { get; set; }

    public bool AfisarePrimaPagina { get; set; }

    public PromovatLevel PromotedLevel { get; set; }
  }
}
