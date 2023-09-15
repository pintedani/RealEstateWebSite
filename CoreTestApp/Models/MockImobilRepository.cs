using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public class MockImobilRepository : IImobilRepository
    {
        public IEnumerable<Imobil> AllImobils 
        {
            get
            {
                var imobils = new List<Imobil>();

                imobils.Add(new Imobil { 
                    Title = "Titlu Anunt", 
                    Descriere = "Descriere Imobil", 
                    Price = 35000,
                    DataAdaugare = DateTime.Today});

                return imobils;
            }
        }

        public IEnumerable<Imobil> LastAddedImobils => throw new NotImplementedException();

        IEnumerable<Imobil> IImobilRepository.AllImobils => throw new NotImplementedException();

        IEnumerable<Imobil> IImobilRepository.LastAddedImobils => throw new NotImplementedException();

        public Imobil GetImobilById(int id)
        {
            return AllImobils.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Imobil> SearchImobils(string searchQuery)
        {
            throw new NotImplementedException();
        }

        Imobil IImobilRepository.GetImobilById(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Imobil> IImobilRepository.SearchImobils(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
