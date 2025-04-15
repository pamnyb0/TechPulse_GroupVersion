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
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,ProfileImageUrl,PhoneNumber,Address,PostalCode,City")] User user, IFormFile profileImage)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                if(profileImage != null && profileImage.Length > 0)
                {
                    var fileName = Path.GetFileName(profileImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(stream);
                    }
                    user.ProfileImageUrl = "/images/" + fileName;
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ett fel uppstod när kontot skulle skapas. Försök igen.");
                return View(user);
            }
        }
        
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}