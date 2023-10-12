using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Logging;
using Microsoft.EntityFrameworkCore;

namespace Imobiliare.Repositories
{
  public class ConstructoriRepository : Repository<Constructor>, IConstructoriRepository
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(ConstructoriRepository));

    public ConstructoriRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Constructor.Id)))
    {
    }

    public List<Constructor> GetConstructori()
    {
      throw new NotImplementedException();
    }

    public Constructor GetConstructor(int id)
    {
      var agentie = this.DbContext.Constructori.Include(nameof(Constructor.ConstructorUserProfiles)).SingleOrDefault(x => x.Id == id);

      return agentie;
    }

    public void Update(Constructor toEditAgentie)
    {
      var agentie = this.DbContext.Constructori.Single(x => x.Id == toEditAgentie.Id);
      agentie.Nume = toEditAgentie.Nume;
      agentie.Descriere = toEditAgentie.Descriere;
      agentie.Telefon = toEditAgentie.Telefon;
      agentie.Website = toEditAgentie.Website;
    }
  }
}
