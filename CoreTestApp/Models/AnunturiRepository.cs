namespace CoreTestApp.Models
{
    public class AnunturiRepository : IImobilRepository
    {
        private readonly ApplicationDbContext _context;

        public AnunturiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Imobil> AllImobils 
        {
            get 
            {
                return _context.Imobils;            
            }
        }

        public IEnumerable<Imobil> LastAddedImobils => throw new NotImplementedException();

        public Imobil GetImobilById(int id)
        {
            return _context.Imobils.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Imobil> SearchImobils(string searchQuery)
        {
            return _context.Imobils.Where(p=>p.Title.Contains(searchQuery));
        }
    }
}
