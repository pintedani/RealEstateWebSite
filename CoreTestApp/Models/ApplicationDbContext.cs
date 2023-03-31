using Microsoft.EntityFrameworkCore;

namespace CoreTestApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Imobil> Imobils { get; set; }
    }
}
