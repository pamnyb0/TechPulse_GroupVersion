using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TechPulse.Data;
using System.Linq;

namespace TechPulse.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly TechPulseDbContext _context;

        public ProfileController(TechPulseDbContext context)
        {
            _context = context;
        }
        public ActionResult Profile()
        {
            var username = User?.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }
            
            var profilepicture = user.ProfileImageUrl ?? "/images/default-picture.jpg";

            ViewData["ProfilePicture"] = profilepicture;
            return View(user);
        }
    }
}
