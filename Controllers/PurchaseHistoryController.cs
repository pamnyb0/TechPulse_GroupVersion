using Microsoft.AspNetCore.Mvc;

namespace TechPulse.Controllers
{
    public class PurchaseHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
