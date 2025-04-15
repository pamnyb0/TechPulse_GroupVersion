using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechPulse.Models;
using TechPulse.Data;
using Microsoft.EntityFrameworkCore;

namespace TechPulse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TechPulseDbContext _context;

        public HomeController(ILogger<HomeController> logger, TechPulseDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Start";
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            ViewBag.Title = "Privacy";
            return View();
        }

        public IActionResult Publish()
        {
            ViewBag.Title = "Publish";
            return View();
        }

        public IActionResult Shop()
        {
            ViewBag.Title = "Shop";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
