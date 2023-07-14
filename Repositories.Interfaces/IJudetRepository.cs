using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.Interfaces
{
    using Imobiliare.Entities;
    public interface IJudetRepository : IRepository<Judet>
    {
        List<Judet> Judete();

        string GetJudetGpsCoordinates(int judetId);

        void Edit(Judet judet);
    }
}
