using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories
{
    public class RaportActivitateRepository : Repository<RaportActivitate>, IRaportActivitateRepository
    {
        public RaportActivitateRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(RaportActivitate.Id), SortDirection.Descending))
        {
        }

        public RaportActivitate GetLastRaportActivitate()
        {
            var raport = this.DbContext.RaportActivitates.OrderByDescending(x=>x.Id).FirstOrDefault();
            return raport;
        }
    }
}
