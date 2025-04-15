using Microsoft.AspNetCore.Mvc;
using TechPulse.Data;
using TechPulse.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TechPulse.Controllers
{
    public class LoginController : Controller
    {
        private readonly TechPulseDbContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(TechPulseDbContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel model)
        {

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Username);
                
                if (user == null)
                {
                    user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                }

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        HttpContext.Session.SetString("IsLoggedIn", "true");
                        HttpContext.Session.SetString("UserId", user.Id.ToString());
                        HttpContext.Session.SetString("Username", user.Username);
                        
                        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                        {
                            HttpContext.Session.SetString("ProfileImageUrl", user.ProfileImageUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Message"] = "Ogiltigt användarnamn eller lösenord.";
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ett fel uppstod vid inloggningen: " + ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}