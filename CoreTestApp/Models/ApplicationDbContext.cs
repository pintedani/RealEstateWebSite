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

        public DbSet<FavoriteAnuntItem> FavoriteAnuntItems { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set;}

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
