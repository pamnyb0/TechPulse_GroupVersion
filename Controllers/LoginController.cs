using Microsoft.AspNetCore.Mvc;
using TechPulse.Data;
using TechPulse.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Try to find user by email first, then by username
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Username);
                if (user == null)
                {
                    user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                }

                if (user != null && user.Password == model.Password)
                {
                    // Set session variables
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("UserRole", user.Role ?? "User");

                    // Set profile image if exists
                    if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                    {
                        HttpContext.Session.SetString("ProfileImageUrl", user.ProfileImageUrl);
                    }

                    _logger.LogInformation($"User {user.Username} logged in successfully");
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Ogiltigt användarnamn eller lösenord");
                _logger.LogWarning($"Failed login attempt for username: {model.Username}");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                ModelState.AddModelError("", "Ett fel uppstod vid inloggningen. Försök igen.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            var username = HttpContext.Session.GetString("Username");
            HttpContext.Session.Clear();
            _logger.LogInformation($"User {username} logged out");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
