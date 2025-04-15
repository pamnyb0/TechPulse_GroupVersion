using Microsoft.EntityFrameworkCore;
using TechPulse.Models;

namespace TechPulse.Data
{
    public class TechPulseDbContext : DbContext
    {
        public TechPulseDbContext(DbContextOptions<TechPulseDbContext> options) : base(options)
        {
        }

        // User and Content Management
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }

        // Product and Shopping System
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure decimal precision for monetary values
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<PurchaseHistory>()
                .Property(ph => ph.TotalAmount)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            // Add test users for development
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 999,
                    Username = "TestUser",
                    Password = "letmein123",
                    Email = "testuser@example.com",
                    PostalCode = "12345",
                    City = "TestCity",
                    ProfileImageUrl = null
                },
                new User
                {
                    Id = 4,
                    Username = "Armend Berisha",
                    Email = "armend.berisha@example.com",
                    Password = "828KD9Ex<*y.",
                    ProfileImageUrl = "741435934740.jpg",
                    PhoneNumber = "070-1234567",
                    Address = "Techgatan 1",
                    PostalCode = "12345",
                    City = "Stockholm"
                }
            );

            // Seed example Articles
            builder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    Title = "Hur du blir produktiv som utvecklare",
                    Content = "Det handlar om vanor, fokus och att använda rätt verktyg.",
                    AuthorId = "TechPulse Team",
                    CreatedAt = new DateTime(2024, 01, 10, 8, 0, 0, DateTimeKind.Utc),
                    IsPublished = true,
                    Category = "Produktivitet"
                },
                new Article
                {
                    Id = 2,
                    Title = "Varför Tailwind CSS är bättre än du tror",
                    Content = "Utility-first CSS låter dig bygga snabbt och konsekvent.",
                    AuthorId = "TechPulse Team",
                    CreatedAt = new DateTime(2024, 01, 08, 12, 0, 0, DateTimeKind.Utc),
                    IsPublished = true,
                    Category = "Frontend"
                }
            );
        }
    }
}
