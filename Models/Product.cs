using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechPulse.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Namn")]
        public string? Name { get; set; }

        [DisplayName("Användarnamn")]
        public string? Username { get; set; }

        [DisplayName("Beskrivning")]
        public string? Description { get; set; }

        [DisplayName("Pris")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DisplayName("Produktbild")]
        public string? ImageUrl { get; set; }
        
        public static Product GetSampleById(int id)
        {
            return id switch
            {
                1 => new Product
                {
                    Id = 1,
                    Name = "Smartphone",
                    Description = "Smartphone",
                    Price = 12999.99m,
                    ImageUrl = "/images/products/smartphone.jpg"
                },
                2 => new Product
                {
                    Id = 2,
                    Name = "Laptop",
                    Description = "En laptop med bra prestanda",
                    Price = 8499.99m,
                    ImageUrl = "/images/products/laptop.jpg"
                },
                3 => new Product
                {
                    Id = 3,
                    Name = "Trådlösa hörlurar",
                    Description = "Trådlösa hörlurar med stor kvalitet",
                    Price = 2799.99m,
                    ImageUrl = "/images/products/headphones.jpg"
                },
                4 => new Product
                {
                    Id = 4,
                    Name = "Smartklocka",
                    Description = "Träningsklocka med smarta funktioner",
                    Price = 3499.99m,
                    ImageUrl = "/images/products/smartwatch.jpg"
                },
                5 => new Product
                {
                    Id = 5,
                    Name = "Datorskärm",
                    Description = "Datormskärm 27 tum",
                    Price = 4299.99m,
                    ImageUrl = "/images/products/monitor.jpg"
                },
                6 => new Product
                {
                    Id = 6,
                    Name = "Gaming mus",
                    Description = "Gaming mus",
                    Price = 899.99m,
                    ImageUrl = "/images/products/mouse.jpg"
                },
                7 => new Product
                {
                    Id = 7,
                    Name = "Gamning tangentbord",
                    Description = "Gaming tangentbord med belysning",
                    Price = 1299.99m,
                    ImageUrl = "/images/products/keyboard.jpg"
                },
                8 => new Product
                {
                    Id = 8,
                    Name = "SSD",
                    Description = "SSD för lagring av olika typer av filer",
                    Price = 1499.99m,
                    ImageUrl = "/images/products/ssd.jpg"
                },
                _ => null
            };
        }
    }
}
