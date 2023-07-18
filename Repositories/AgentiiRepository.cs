using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Imobiliare.Repositories
{
    public class AgentiiRepository : Repository<Agentie>, IAgentiiRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UsersRepository));

        public AgentiiRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Agentie.Id)))
        {
            this.IncludeList.Add("AgentieImobiliaraUserProfiles.Anunturi");
            this.IncludeList.Add(nameof(Agentie.Oras));
        }

        public List<Agentie> GetAgentii()
        {
            return this.DbContext.Agenties.Include(nameof(Agentie.Oras)).Include(nameof(Agentie.AgentieImobiliaraUserProfiles)).ToList();
        }

        public List<Agentie> GetAgenties(AgentiiImobiliareFilter filter, out int totalNumberOfPages)
        {
            var query = this.DbContext.Agenties.Include(nameof(Agentie.Oras))
                .Include(nameof(Agentie.AgentieImobiliaraUserProfiles));

            var allFilteredImobils =
                query.OrderByDescending(x => x.Id)
                    .Where((x => x.OrasSelect == null || x.OrasSelect == filter.OrasId));

            totalNumberOfPages = allFilteredImobils.Count();
            return
                allFilteredImobils.Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();
        }

        public List<Agentie> GetAgentiiForOras(int orasId, bool onlyWithPoza)
        {
            return
              this.DbContext.Agenties.Include(nameof(Agentie.AgentieImobiliaraUserProfiles))
                .Include("AgentieImobiliaraUserProfiles.Anunturi")
                .Include(nameof(Agentie.Oras))
                .Where(x => x.OrasSelect == orasId && (onlyWithPoza != true || !string.IsNullOrEmpty(x.PozaAgentie)))
                .ToList();
        }

        public void Update(Agentie toEditAgentie)
        {
            var agentie = this.DbContext.Agenties.Include(nameof(Agentie.Oras)).Single(x => x.Id == toEditAgentie.Id);
            agentie.Nume = toEditAgentie.Nume;
            agentie.DescriereAgentie = toEditAgentie.DescriereAgentie;
            agentie.TelefonAgentie = toEditAgentie.TelefonAgentie;
            agentie.Website = toEditAgentie.Website;
            agentie.AfisarePrimaPagina = toEditAgentie.AfisarePrimaPagina;
            if (toEditAgentie.OrasSelect.HasValue && toEditAgentie.OrasSelect != 0)
            {
                agentie.Oras = this.DbContext.Orase.Single(x => x.Id == toEditAgentie.OrasSelect.Value);
            }
            else
            {
                //In caz ca s-a scos orasul de la agentie
                agentie.OrasSelect = null;
                agentie.Oras = null;
            }
        }

        public void Delete(Agentie toDeleteAgentie)
        {
            var agentie = this.DbContext.Agenties.Include(nameof(Agentie.AgentieImobiliaraUserProfiles)).Single(x => x.Id == toDeleteAgentie.Id);
            if(agentie.AgentieImobiliaraUserProfiles.Count > 0)
            {
                throw new NotSupportedException("Agentie has users, cannot delete unless all users are deleted manually");
            }

            DbContext.Agenties.Remove(agentie);
            DeleteAllAgentiePhotos(agentie.PozaAgentie);
        }

        private void DeleteAllAgentiePhotos(string poza)
        {
            if (poza != null)
            {
                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/LogoAgentii"), poza);
                var fileDel = new FileInfo(path);
                if (fileDel.Exists)
                {
                    fileDel.Delete();
                    log.DebugFormat("Deleted agentie photo {0}", poza);
                }
                else
                {
                    log.ErrorFormat("Attempt to remove inexisting agentie photo {0}", poza);
                }
            }
        }
    }
}
