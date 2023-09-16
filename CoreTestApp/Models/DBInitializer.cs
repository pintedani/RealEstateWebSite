using Imobiliare.Repositories;

namespace Imobiliare.UI.Models
{
    public static class DBInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            ApplicationDbContext applicationDbContext = 
                applicationBuilder.ApplicationServices.CreateScope().
                ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //if(!applicationDbContext.Imobile.Any()) 
            //{
                //applicationDbContext.Imobils.Add(new Imobil()
                //{
                //    Title = "Apartament 1",
                //    Descriere = "Descriere Ap 1",
                //    Price = 35000,
                //    DataAdaugare = DateTime.Now,
                //    DataAdaugareInitiala = DateTime.Now,
                //    ContactTelefon = "0744577632",
                //    Suprafata = 39
                //});

                //applicationDbContext.Imobils.Add(new Imobil()
                //{
                //    Title = "Apartament 2",
                //    Descriere = "Descriere Ap 2",
                //    Price = 50000,
                //    DataAdaugare = DateTime.Now,
                //    DataAdaugareInitiala = DateTime.Now,
                //    ContactTelefon = "0746377677",
                //    Suprafata = 55
                //});

                //applicationDbContext.Imobils.Add(new Imobil()
                //{
                //    Title = "Apartament 3",
                //    Descriere = "Descriere Ap 3",
                //    Price = 80000,
                //    DataAdaugare = DateTime.Now,
                //    DataAdaugareInitiala = DateTime.Now,
                //    ContactTelefon = "0746377888",
                //    Suprafata = 80
                //});

                //applicationDbContext.SaveChanges();
            //}
        }
    }
}
