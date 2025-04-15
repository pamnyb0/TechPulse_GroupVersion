using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechPulse.Data;
using TechPulse.Models;
using Microsoft.Extensions.Logging;

namespace TechPulse.Controllers
{
    public class CreateAccountController : Controller
    {
        private readonly TechPulseDbContext _context;
        private readonly ILogger<CreateAccountController> _logger;

        public CreateAccountController(TechPulseDbContext context, ILogger<CreateAccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,ProfileImageUrl,PhoneNumber,Address,PostalCode,City")] User user, IFormFile? profileImage)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                // Check if username or email already exists
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Användarnamnet är redan taget.");
                    return View(user);
                }

                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "E-postadressen är redan registrerad.");
                    return View(user);
                }

                // Handle profile image upload
                if (profileImage != null && profileImage.Length > 0)
                {
                    try
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
                        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        
                        // Create uploads directory if it doesn't exist
                        if (!Directory.Exists(uploadsPath))
                        {
                            Directory.CreateDirectory(uploadsPath);
                        }

                        var filePath = Path.Combine(uploadsPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await profileImage.CopyToAsync(stream);
                        }
                        user.ProfileImageUrl = $"~/uploads/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error uploading profile image: {ex.Message}");
                        ModelState.AddModelError("ProfileImageUrl", "Det gick inte att ladda upp profilbilden. Försök igen.");
                        return View(user);
                    }
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"New user account created: {user.Username}");

                // Automatically log in the user after account creation
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    HttpContext.Session.SetString("ProfileImageUrl", user.ProfileImageUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating account: {ex.Message}");
                ModelState.AddModelError("", "Ett fel uppstod när kontot skulle skapas. Försök igen.");
                return View(user);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Only allow users to edit their own profile
            if (HttpContext.Session.GetString("UserId") != id.ToString())
            {
                return Forbid();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password,ProfileImageUrl,PhoneNumber,Address,PostalCode,City")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            // Only allow users to edit their own profile
            if (HttpContext.Session.GetString("UserId") != id.ToString())
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Check if username is changed and not taken
                    if (user.Username != existingUser.Username && 
                        await _context.Users.AnyAsync(u => u.Username == user.Username))
                    {
                        ModelState.AddModelError("Username", "Användarnamnet är redan taget.");
                        return View(user);
                    }

                    // Check if email is changed and not taken
                    if (user.Email != existingUser.Email && 
                        await _context.Users.AnyAsync(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "E-postadressen är redan registrerad.");
                        return View(user);
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"User account updated: {user.Username}");

                    // Update session data
                    HttpContext.Session.SetString("Username", user.Username);
                    if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                    {
                        HttpContext.Session.SetString("ProfileImageUrl", user.ProfileImageUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await _context.Users.AnyAsync(u => u.Id == user.Id))
                    {
                        return NotFound();
                    }
                    
                    _logger.LogError($"Concurrency error updating user {user.Username}: {ex.Message}");
                    ModelState.AddModelError("", "Ett fel uppstod när profilen skulle uppdateras. Försök igen.");
                }
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
