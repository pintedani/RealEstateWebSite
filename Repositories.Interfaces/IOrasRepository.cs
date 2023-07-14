using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.Interfaces
{
  using Imobiliare.Entities;
  public interface IOrasRepository : IRepository<Oras>
  {
    List<Oras> GetSelectableOrases(int judetId);

    Dictionary<Oras, int> GetSelectableOrasesWithNumber(ImobilFilter imobilFilter);

    Dictionary<int, string> GetMatchingLocations(string searchText, bool includingTotJudet);

    string GetOrasGpsCoordinates(int orasId);

    bool AddLocalitate(string nume, int judetId, string coordinateGps, bool resedintaJudet, bool localitateMica);

    bool DeleteLocalitate(int orasID);

    bool EditeazaLocalitate(int orasID, string coordinate, string nume, bool resedintaJudet, bool localitateMica);

    Oras GetUniqueSelectableOras(string searchValue);

    bool UpdateDescriptionLocalitate(int localitateId, string localitateDescriere);
  }
}
