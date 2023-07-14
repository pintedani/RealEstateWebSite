using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;

namespace Imobiliare.Repositories.Interfaces
{
  public interface IBlockedIpRepository : IRepository<BlockedIp>
  {
    int UpdateHitCount(string ipAddr);
  }
}
