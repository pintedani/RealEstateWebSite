using Caching;

namespace Imobiliare.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Entities;
    using Interfaces;

    public class OrasRepository : Repository<Oras>, IOrasRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrasRepository));

        private IJudetRepository judetRepository;

        public OrasRepository(ApplicationDbContext dbContext, IJudetRepository judetRepository)
          : base(dbContext, new SortSpec(nameof(Oras.Id)))
        {
            this.judetRepository = judetRepository;
        }

        public List<Oras> GetSelectableOrases(int judetId)
        {
            var orases = this.DbContext.Orase.Where(x => x.JudetId == judetId);
            return orases.OrderBy(x => x.Nume).ToList();
        }

        public Dictionary<Oras, int> GetSelectableOrasesWithNumber(ImobilFilter imobilFilter)
        {
            var selectableOrasesWithNumber = new Dictionary<Oras, int>();
            DateTime dateLimit = DateTime.Now;
            if (imobilFilter.VechimeMaximaZile > 0)
            {
                dateLimit = DateTime.Now.AddDays(-imobilFilter.VechimeMaximaZile);
            }

            var orases = this.DbContext.Orase.Include("Judet").Where(x => x.JudetId == imobilFilter.JudetId).OrderBy(x => x.Nume);
            foreach (var oras in orases.ToList())
            {
                //to satisfy linq
                var oras1 = oras;
                selectableOrasesWithNumber.Add(oras1, 0);
            }

            foreach (var imobil in this.DbContext.Imobile.Include("Oras").Where(x => x.StareAprobare == StareAprobare.Aprobat
                    && (imobilFilter.TipProprietate == TipProprietate.Toate || imobilFilter.TipProprietate == x.TipProprietate)
                    && (imobilFilter.TipOfertaGen == TipOfertaGen.Toate || imobilFilter.TipOfertaGen == x.TipOfertaGen)
                    && (imobilFilter.CamereMin == 0 || x.NumarCamere >= imobilFilter.CamereMin)
                    && (imobilFilter.CamereMax == 0 || x.NumarCamere <= imobilFilter.CamereMax)
                    && (imobilFilter.MpMin == 0 || x.Suprafata >= imobilFilter.MpMin)
                    && (imobilFilter.MpMax == 0 || x.Suprafata <= imobilFilter.MpMax)
                    && (imobilFilter.PretMin == 0 || x.Price >= imobilFilter.PretMin)
                    && (imobilFilter.PretMax == 0 || x.Price <= imobilFilter.PretMax)
                    && (x.TipVanzator == imobilFilter.TipVanzator || imobilFilter.TipVanzator == TipVanzator.TotiVanzatorii)
                    && (imobilFilter.VechimeMaximaZile == 0 || x.DataAdaugare >= dateLimit)))
            {
                if (selectableOrasesWithNumber.Any(x => x.Key.Id == imobil.Oras.Id))
                {
                    var oras = selectableOrasesWithNumber.Single(x => x.Key.Id == imobil.Oras.Id).Key;
                    selectableOrasesWithNumber[oras]++;
                }
            }

            return selectableOrasesWithNumber;
        }

        public bool AddLocalitate(string nume, int judetId, string coordinateGps, bool resedintaJudet, bool localitateMica)
        {
            try
            {
                if (nume == "0" || nume == string.Empty || coordinateGps == string.Empty)
                {
                    throw new ArgumentException("Name or coordinate is null");
                }
                if (!this.DbContext.Judete.Any(x => x.Id == judetId))
                {
                    throw new ArgumentException("No judet with id " + judetId);
                }
                if (this.DbContext.Orase.Any(x => x.JudetId == judetId && x.Nume == nume))
                {
                    throw new ArgumentException("Duplicate localitate name for same judet");
                }
                this.DbContext.Orase.Add(new Oras() { Nume = nume, CoordinateGps = coordinateGps, JudetId = judetId, ResedintaJudet = resedintaJudet, LocalitateMica = localitateMica });
                return true;
            }
            catch (Exception exception)
            {
                log.ErrorFormat("Error while adding localitate {0} for judet with id {1}, exception {2}", nume, judetId, exception);
                return false;
            }
        }

        public bool DeleteLocalitate(int orasID)
        {
            var oras = this.DbContext.Orase.Single(e => e.Id == orasID);
            try
            {
                this.DbContext.Orase.Remove(oras);
            }
            catch (Exception exception)
            {
                string error = exception.Message;
                if (exception.InnerException != null)
                {
                    error += exception.InnerException.Message;
                }
                log.ErrorFormat("Error while deleting oras with id {0}, exception {1}", orasID, error);
                return false;
            }
            return true;
        }

        public bool EditeazaLocalitate(int orasID, string coordinate, string nume, bool resedintaJudet, bool localitateMica)
        {
            try
            {
                var localitate = this.DbContext.Orase.Single(x => x.Id == orasID);
                localitate.Nume = nume;
                localitate.CoordinateGps = coordinate;
                localitate.ResedintaJudet = resedintaJudet;
                localitate.LocalitateMica = localitateMica;
            }
            catch (Exception exception)
            {
                string error = exception.Message;
                if (exception.InnerException != null)
                {
                    error += exception.InnerException.Message;
                }
                log.ErrorFormat("Error while editing localitate with id {0}, exception {1}", orasID, error);
                return false;
            }
            return true;
        }

        public Oras GetUniqueSelectableOras(string searchValue)
        {
            Oras returnedOras = null;
            searchValue = new string(searchValue.Where(Char.IsLetter).ToArray());
            foreach (var oras in this.DbContext.Orase)
            {
                var formattedOras = new string(oras.Nume.Where(Char.IsLetter).ToArray());
                if (formattedOras.IndexOf(searchValue, StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    //Get first entry for now
                    returnedOras = oras;
                    break;
                }
            }
            return returnedOras;
        }

        public bool UpdateDescriptionLocalitate(int localitateId, string localitateDescriere)
        {
            var anunt = this.DbContext.Orase.FirstOrDefault(x => x.Id == localitateId);
            if (anunt != null)
            {
                anunt.DescriereLocalitate = localitateDescriere;
            }

            return true;
        }

        public string GetOrasGpsCoordinates(int orasId)
        {
            var query = this.DbContext.Orase.Single(x => x.Id == orasId);
            var coord = query.CoordinateGps;
            return coord;
        }

        public Dictionary<int, string> GetMatchingLocations(string searchText, bool includingTotJudet)
        {
            var matchingLocations = new Dictionary<int, string>();

            if (searchText != string.Empty)
            {
                try
                {
                    foreach (var oras in this.GetOrase())
                    {
                        if (oras.Nume != "Alta Localitate"
                            && this.GetStringWithoutSpecialChars(oras.Nume)
                              .IndexOf(this.GetStringWithoutSpecialChars(searchText), StringComparison.InvariantCultureIgnoreCase)
                            != -1)
                        {
                            var judet = this.judetRepository.Judete().First(x => x.Id == oras.JudetId);
                            matchingLocations.Add(oras.Id, oras.Nume + " | " + judet.PrescurtareAuto);

                            //Show max 30
                            if (matchingLocations.Count > 30)
                            {
                                break;
                            }
                        }
                    }
                    //For user searching include also "tot judetul"
                    if (includingTotJudet)
                    {
                        foreach (var judet in this.judetRepository.Judete())
                        {
                            if (this.GetStringWithoutSpecialChars(judet.Nume)
                                  .IndexOf(this.GetStringWithoutSpecialChars(searchText), StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                //TODO DAPI: Check here when id ovelaps with that of orase - Error Logs:  Search results error for text s with error An item with the same key has already been added. | Imobiliare.Repositories.OptionsRepository
                                if (!matchingLocations.ContainsKey(judet.Id))
                                {
                                    matchingLocations.Add(judet.Id, judet.Nume + " (tot judetul)");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Search results error for text {0} with error {1}", searchText, ex.Message);
                }
            }

            return matchingLocations;
        }

        private string GetStringWithoutSpecialChars(string stringToFormat)
        {
            var result = Regex.Replace(stringToFormat, "[^.a-z,A-Z]", "");
            return result;
        }

        private List<Oras> GetOrase()
        {
            if (LocatiiCaching.Localitati != null)
            {
                return LocatiiCaching.Localitati;
            }
            var orase = this.DbContext.Orase.ToList();

            LocatiiCaching.Localitati = orase;

            return orase;
        }
    }
}
