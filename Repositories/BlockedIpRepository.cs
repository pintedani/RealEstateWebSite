using System.Linq;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;

namespace Imobiliare.Repositories
{
    public class BlockedIpRepository : Repository<BlockedIp>, IBlockedIpRepository
    {
        public BlockedIpRepository(ApplicationDbContext dbContext)
          : base(dbContext, new SortSpec(nameof(BlockedIp.Id)))
        {
        }

        public int UpdateHitCount(string ipAddr)
        {
            var itemToremove = this.DbContext.BlockedIps.FirstOrDefault(x => x.Address == ipAddr);
            if (itemToremove != null)
            {
                itemToremove.BlockCount++;
                var total = itemToremove.BlockCount;
                return total;
            }
            return 0;
        }
    }
}
