using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public IActionResult OrganizerLogin()
        {
            return View();
        }
        public IActionResult OrganizerRegister()
        {
            return View();
        }
        
        // GET: 活動方忘記密碼頁面
        public ActionResult OrganizerForgetPwd()
        {
            return View();
        }

        public IActionResult OrganizerInfo()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //mainModal
        public IActionResult mainModal()
        {
            return PartialView("_mainModalPartial");
        }
        //mainModal

        //管理員Session設定
        [HttpPost]
        public IActionResult isMG(string key)
        {
            if (key == "true")
            {
                HttpContext.Session.SetString("isMg", key);
            }
            else
            {
                HttpContext.Session.Remove("isMg");
            }
            return Ok();
        }
    }
}
