using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.Interfaces
{
  using Imobiliare.Entities;
  public interface ICartierRepository : IRepository<Cartier>
  {
    Dictionary<Cartier, int> GetSelectableCartiersWithNumber(ImobilFilter imobilFilter);

    List<Cartier> GetSelectableCartiers(int orasId);

    bool DeleteLastCartier(int cartierID);

    bool AddCartier(string nume, int orasId);

    bool EditeazaCartier(int cartierID, string nume);
  }
}
