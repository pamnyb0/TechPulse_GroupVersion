using Microsoft.AspNetCore.Mvc;
using TechPulse.Data;
using TechPulse.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TechPulse.Controllers
{
    public class LoginController : Controller
    {
        private readonly TechPulseDbContext _context;

        public LoginController(TechPulseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Set session variables for logged in user
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("Username", user.Username);
                    
                    // Set profile image URL, using the uploads directory
                    if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                    {
                        HttpContext.Session.SetString("ProfileImageUrl", $"~/uploads/{user.ProfileImageUrl}");
                    }

                    return RedirectToAction("Index", "Home");
                }

                TempData["Message"] = "Ogiltigt användarnamn eller lösenord.";
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear all session variables
            HttpContext.Session.Clear();
            
            // Redirect to home page
            return RedirectToAction("Index", "Home");
        }
    }
}
