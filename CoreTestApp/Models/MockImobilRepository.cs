﻿namespace CoreTestApp.Models
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

        public Imobil GetImobilById(int id)
        {
            return AllImobils.FirstOrDefault(x => x.Id == id);
        }
    }
}
