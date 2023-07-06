using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreTestApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<UserProfile>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Imobil> Imobils { get; set; }

        public DbSet<Judet> Judete { get; set; }
        public DbSet<Oras> Orase { get; set; }
        public DbSet<Cartier> Cartiere { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Stire> Stires { get; set; }
        public DbSet<Agentie> Agenties { get; set; }
        public DbSet<Constructor> Constructori { get; set; }

        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<BlockedIp> BlockedIps { get; set; }
        public DbSet<Mesaj> Mesaje { get; set; }

        public DbSet<SystemSettings> SystemSettings { get; set; }
        //public DbSet<Ansamblu> Ansambluri { get; set; }

        public DbSet<RaportActivitate> RaportActivitates { get; set; }

        public DbSet<FavoriteAnuntItem> FavoriteAnuntItems { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set;}

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
