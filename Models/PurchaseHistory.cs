using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechPulse.Models
{
    public class PurchaseHistory
    {
        public int Id { get; set; } // Unique identifier for the purchase history
        [Required]
        public int UserId { get; set; } // Foreign key to the user who made the purchase
        public User? User { get; set; } // Navigation property to the associated user
        [Required]
        public int ProductId { get; set; } // Foreign key to the purchased product
        public Product? Product { get; set; } // Navigation property to the associated product
        [Required]
        [DisplayName("Köpdatum")]
        [DataType(DataType.DateTime)]
        public DateTime PurchaseDate { get; set; } // Date and time of the purchase
        [Required]
        [DisplayName("Totalt belopp")]
        [DataType(DataType.Currency)]
        [Range(0, 999999.99, ErrorMessage = "Beloppet måste vara mellan 0 och 999,999.99 kr")]
        public decimal TotalAmount { get; set; } // Total amount of the purchase
    }
}