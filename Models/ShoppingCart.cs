using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TechPulse.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var session = httpContextAccessor?.HttpContext?.Session;
            var httpContext = httpContextAccessor?.HttpContext;
            
            if (session?.TryGetValue("CartCleared", out _) == true || 
                session?.GetString("ForceEmptyCart") == "true")
            {
                return new ShoppingCart();
            }
            
            var cart = session?.GetString("Cart") ?? string.Empty;
            return string.IsNullOrEmpty(cart) 
                ? new ShoppingCart() 
                : JsonConvert.DeserializeObject<ShoppingCart>(cart) ?? new ShoppingCart();
        }

        public void SaveToSession(ISession session)
        {
            session.SetString("Cart", JsonConvert.SerializeObject(this));
        }

        public void AddItem(Product product, int quantity = 1)
        {
            var cartItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
            
            if (cartItem == null)
            {
                Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity,
                    Price = product.Price
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal GetTotal()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
} 