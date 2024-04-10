using Microsoft.AspNetCore.Mvc;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class RouletteController : Controller
    {
        private readonly ILogger<RouletteController> _loggerRoulette;
        public RouletteController(ILogger<RouletteController> logger)
        {
            _loggerRoulette = logger;
        }
        public IActionResult RouletteIndex()
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
