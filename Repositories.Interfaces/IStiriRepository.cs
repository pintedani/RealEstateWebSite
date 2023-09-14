using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.Repositories.Interfaces
{
    public interface IStiriRepository : IRepository<Stire>
    {
        List<Stire> GetLastAddedStiri();

        void Edit(Stire stire);

        void Delete(int id);

        void Add(Stire stire);

        //string AddImage(int stireId, IFormFile[] IFormFiles);

        void DeleteImage(int stireId, string poza);

        Tuple<Stire, Stire> FindBeforeAndAfterNavigationStiri(int id, StiriFilter stiriFilter);

        Stire TryGetCachedStire(int id);
    }
}
