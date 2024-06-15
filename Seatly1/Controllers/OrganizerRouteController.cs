using Microsoft.AspNetCore.Mvc;

namespace Seatly1.Controllers
{
    public class OrganizerRouteController : Controller
    {
        // 活動方主頁
        public IActionResult Index()
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
        public IActionResult OrganizerForgetPwd()
        {
            return View();
        }

        public IActionResult OrganizerInfo()
        {
            return View();
        }
        public IActionResult NotificationRecord()
        {
            return View();
        }
        public IActionResult ActivityCreate()
        {
            return View();
        }
        public IActionResult ActivityEdit()
        {
            return View();
        }
        // 進入個別活動頁面
        public IActionResult Activity()
        {
            return View();
        }
        public IActionResult Description()
        {
            return View();
        }
    }
}
