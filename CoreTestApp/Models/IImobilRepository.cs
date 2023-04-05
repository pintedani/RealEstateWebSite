namespace CoreTestApp.Models
{
    public interface IImobilRepository
    {
        IEnumerable<Imobil> AllImobils { get; }

        IEnumerable<Imobil> LastAddedImobils { get; }

        Imobil GetImobilById(int id);

        IEnumerable<Imobil> SearchImobils(string searchQuery);
    }
}
