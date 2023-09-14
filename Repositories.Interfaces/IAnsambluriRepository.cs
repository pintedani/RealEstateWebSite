using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;

namespace Imobiliare.Repositories.Interfaces
{
    using System.Web;

    public interface IAnsambluriRepository : IRepository<Ansamblu>
    {
        List<Ansamblu> GetAnsambluri(AnsambluriRezidentialeFilter filter, out int totalNumberOfPages);
  
        void Delete(int id);

        void Edit(Ansamblu ansamblu);

        //string AddImage(int ansambluId, IFormFile[] IFormFiles);

        void DeleteImage(int ansambluId, string poza);

        void IncrementNumarAccesari(int ansambluIdValue);
    }
}
