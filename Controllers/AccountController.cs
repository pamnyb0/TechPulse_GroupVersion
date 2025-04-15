using Microsoft.AspNetCore.Mvc;
using TechPulse.Data;
using TechPulse.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TechPulse.Controllers
{
    public class AccountController : Controller
    {
        private readonly TechPulseDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(TechPulseDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "E-post och lösenord måste anges." });
            }

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email || u.Username == email);

                if (user != null)
                {
                    if (user.Password == password)
                    {
                        HttpContext.Session.SetString("IsLoggedIn", "true");
                        HttpContext.Session.SetString("UserId", user.Id.ToString());
                        HttpContext.Session.SetString("Username", user.Username);

                        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                        {
                            HttpContext.Session.SetString("ProfileImageUrl", user.ProfileImageUrl);
                        }

                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Ogiltigt lösenord." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Användaren finns inte." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ett fel uppstod vid inloggningen. Försök igen." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Login");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userId, out int id))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            try
            {
                var orders = _context.Orders
                    .Where(o => o.UserId == id)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
                
                foreach (var order in orders)
                {
                    if (!string.IsNullOrEmpty(order.CartData))
                    {
                        try 
                        {
                            var items = JsonConvert.DeserializeObject<List<dynamic>>(order.CartData);
                            order.OrderItems = new List<OrderItem>();
                            
                            foreach (var item in items)
                            {
                                order.OrderItems.Add(new OrderItem
                                {
                                    OrderId = order.Id,
                                    ProductId = (int)item.ProductId,
                                    ProductName = (string)item.ProductName,
                                    Quantity = (int)item.Quantity,
                                    UnitPrice = (decimal)item.UnitPrice
                                });
                            }
                        }
                        catch
                        {
                            order.OrderItems = new List<OrderItem>();
                        }
                    }
                    else
                    {
                        order.OrderItems = _context.OrderItems
                            .Where(oi => oi.OrderId == order.Id)
                            .ToList();
                        
                        foreach (var item in order.OrderItems)
                        {
                            if (string.IsNullOrEmpty(item.ProductName) || item.UnitPrice == 0)
                            {
                                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId) 
                                              ?? Product.GetSampleById(item.ProductId);
                                              
                                if (product != null)
                                {
                                    item.ProductName = string.IsNullOrEmpty(item.ProductName) ? product.Name : item.ProductName;
                                    item.UnitPrice = item.UnitPrice == 0 ? product.Price : item.UnitPrice;
                                }
                            }
                        }
                    }
                }

                ViewBag.Orders = orders;
            }
            catch (Exception ex)
            {
                ViewBag.Orders = new List<Order>();
                TempData["ErrorMessage"] = "Det gick inte att ladda orderhistoriken. Försök igen senare.";
            }
            
            return View(user);
        }
    }
} 