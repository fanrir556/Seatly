using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return PartialView("~/Views/Admin/_NotificationRecord-Index.cshtml", notificationRecords);
        }





        public IActionResult _AspNetUsers()
        {
            var _AspNetUsers = _context.AspNetUsers.ToList();
            return PartialView("~/Views/Admin/_AspNetUsers-Index.cshtml", _AspNetUsers);
        }




        public IActionResult _OrganizersAdmin()
        {
            var _OrganizersAdmin = _context.Organizers.ToList();
            return PartialView("~/Views/Admin/_OrganizersAdmin-Index.cshtml", _OrganizersAdmin);
        }
      





    }
}
