using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechPulse.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        
        [Required]
        [DisplayName("Orderdatum")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        [Required]
        [DisplayName("Totalt belopp")]
        [DataType(DataType.Currency)]
        [Range(0, 999999.99, ErrorMessage = "Beloppet måste vara mellan 0 och 999,999.99 kr")]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public string CartData { get; set; } = string.Empty;
        
        public List<OrderItem>? OrderItems { get; set; }
    }
    
    public class OrderItem
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
        [Required]
        [DisplayName("Produktnamn")]
        [StringLength(100, ErrorMessage = "Produktnamnet får inte vara längre än 100 tecken")]
        public string ProductName { get; set; } = string.Empty;
        
        [Required]
        [DisplayName("Antal")]
        [Range(1, 100, ErrorMessage = "Antalet måste vara mellan 1 och 100")]
        public int Quantity { get; set; }
        
        [Required]
        [DisplayName("Styckpris")]
        [DataType(DataType.Currency)]
        [Range(0, 999999.99, ErrorMessage = "Priset måste vara mellan 0 och 999,999.99 kr")]
        public decimal UnitPrice { get; set; }
    }
} 