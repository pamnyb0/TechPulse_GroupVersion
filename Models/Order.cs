using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechPulse.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User? User { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        public decimal TotalAmount { get; set; }
        
        public string CartData { get; set; } = string.Empty;
        
        public List<OrderItem>? OrderItems { get; set; }
    }
    
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
        public string ProductName { get; set; } = string.Empty;
        
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
    }
} 