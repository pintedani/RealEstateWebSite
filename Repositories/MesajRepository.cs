using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;

namespace Imobiliare.Repositories
{
  public class MesajRepository : Repository<Mesaj>, IMesajRepository
  {
    public MesajRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Mesaj.Id), SortDirection.Descending))
    {
      this.IncludeList.Add(nameof(Mesaj.FromUser));
      this.IncludeList.Add(nameof(Mesaj.ToUser));
      this.IncludeList.Add(nameof(Mesaj.Imobil));
      this.IncludeList.Add(nameof(Mesaj.UserContactForm));
    }
  }
}
