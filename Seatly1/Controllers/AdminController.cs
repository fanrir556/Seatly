using Microsoft.AspNetCore.Mvc;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    public class adminController : Controller
    {
        private readonly SeatlyContext _context;

        public adminController(SeatlyContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _NotificationRecord()
        {
            var notificationRecords = _context.NotificationRecords.ToList();
            return PartialView("_NotificationRecord", notificationRecords);
        }



    }
}
