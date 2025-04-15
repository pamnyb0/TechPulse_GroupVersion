using Microsoft.AspNetCore.Mvc;
using TechPulse.Data;
using TechPulse.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TechPulse.Controllers
{
    public class CartController : Controller
    {
        private readonly TechPulseDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(TechPulseDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("ForceEmptyCart") == "true")
            {
                HttpContext.Session.Remove("ForceEmptyCart");
                
                return View(new ShoppingCart());
            }
            
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = Product.GetSampleById(id);
            
            if (product == null)
            {
                return NotFound();
            }
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            
            cart.AddItem(product);
            
            cart.SaveToSession(HttpContext.Session);
            
            return RedirectToAction("Index", "Product");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            
            cart.RemoveItem(id);
            
            cart.SaveToSession(HttpContext.Session);
            
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index");
            }
            
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Du måste logga in för att slutföra köpet." });
                }
                
                return RedirectToAction("Login", "Account");
            }
            
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            if (cart.Items.Count == 0)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Din varukorg är tom." });
                }
                
                return RedirectToAction("Index");
            }
            
            int userId;
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out userId))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Kunde inte hämta användarinformation. Logga in igen." });
                }
                
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var enrichedCartItems = new List<object>();
                foreach (var item in cart.Items)
                {
                    var product = item.Product ?? Product.GetSampleById(item.ProductId);
                    
                    enrichedCartItems.Add(new
                    {
                        ProductId = item.ProductId,
                        ProductName = product?.Name ?? $"Produkt #{item.ProductId}",
                        UnitPrice = product?.Price ?? item.Price,
                        Quantity = item.Quantity,
                        TotalPrice = (product?.Price ?? item.Price) * item.Quantity,
                        ImageUrl = product?.ImageUrl ?? string.Empty
                    });
                }
                
                string cartJson = JsonConvert.SerializeObject(enrichedCartItems);
                
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.GetTotal(),
                    CartData = cartJson
                };
                
                _context.Orders.Add(order);
                _context.SaveChanges();
                
                ClearShoppingCart();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                
                return RedirectToAction("Profile", "Account");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = false, 
                        message = $"Ett fel uppstod när din beställning skulle läggas: {ex.Message}" 
                    });
                }
                
                return RedirectToAction("Checkout");
            }
        }

        private void ClearShoppingCart()
        {
            var cart = ShoppingCart.GetCart(HttpContext.RequestServices);
            cart.Clear();
            cart.SaveToSession(HttpContext.Session);
            HttpContext.Session.SetString("ForceEmptyCart", "true");
        }
    }
} 