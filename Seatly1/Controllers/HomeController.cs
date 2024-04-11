using Microsoft.AspNetCore.Mvc;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PointsShop()
        {
            return View();
        }

        public IActionResult Coupon()
        {
            return View();
        }

        public IActionResult PointsHistory()
        {
            return View();
        }
        public IActionResult OrganizerLogin()
        {
            return View();
        }
        public IActionResult OrganizerRegister()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
