using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;

namespace Imobiliare.Repositories.Interfaces
{
    public interface IAgentiiRepository : IRepository<Agentie>
    {
        List<Agentie> GetAgentii();

        List<Agentie> GetAgenties(AgentiiImobiliareFilter filter, out int totalNumberOfPages);

        List<Agentie> GetAgentiiForOras(int orasId, bool onlyWithPoza);

        void Update(Agentie toEditAgentie);

        void Delete(Agentie agentie);
    }
}
