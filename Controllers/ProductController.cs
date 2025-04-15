using Microsoft.AspNetCore.Mvc;
using TechPulse.Data;
using TechPulse.Models;
using System.Collections.Generic;
using System.Linq;

namespace TechPulse.Controllers
{
    public class ProductController : Controller
    {
        private readonly TechPulseDbContext _context;

        public ProductController(TechPulseDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = new List<Product>
            {
                Product.GetSampleById(1),
                Product.GetSampleById(2),
                Product.GetSampleById(3),
                Product.GetSampleById(4),
                Product.GetSampleById(5),
                Product.GetSampleById(6),
                Product.GetSampleById(7),
                Product.GetSampleById(8)
            };
            
            products = products.Where(p => p != null).ToList();
            
            return View(products);
        }
    }
} 