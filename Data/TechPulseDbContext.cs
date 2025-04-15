using Microsoft.EntityFrameworkCore;
using TechPulse.Models;

namespace TechPulse.Data
{
    public class TechPulseDbContext : DbContext
    {
        public TechPulseDbContext(DbContextOptions<TechPulseDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
