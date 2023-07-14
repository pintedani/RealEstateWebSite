using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.Interfaces
{
    public interface IRaportActivitateRepository : IRepository<RaportActivitate>
    {
        RaportActivitate GetLastRaportActivitate();
    }
}
