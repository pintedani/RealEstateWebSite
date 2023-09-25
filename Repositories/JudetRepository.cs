using Caching;

namespace Imobiliare.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class JudetRepository : Repository<Judet>, IJudetRepository
    {
        public JudetRepository(ApplicationDbContext dbContext)
          : base(dbContext, new SortSpec(nameof(Judet.Id)))
        {
        }

        public List<Judet> Judete()
        {
            if (LocatiiCaching.Judete != null)
            {
                return LocatiiCaching.Judete;
            }

            //var emails = this.DbContext.EmailTemplates.ToList();
            var judete = this.DbContext.Judete.ToList();

            LocatiiCaching.Judete = judete;

            return judete;
        }

        public string GetJudetGpsCoordinates(int judetId)
        {
            var query = this.DbContext.Judete.Single(x => x.Id == judetId);
            var coord = query.CoordinateGps;
            return coord;
        }

        public void Edit(Judet judet)
        {
            this.DbContext.Entry(judet).State = EntityState.Modified;
        }
    }
}
