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
            return PartialView("~/Views/Admin/NotificationRecord/_index.cshtml", notificationRecords);
        }

        public IActionResult _AspNetUsers()
        {
            var AspNetUsers = _context.AspNetUsers.ToList();
            return PartialView("~/Views/AspNetUsers/index.cshtml", AspNetUsers);
        }

        public IActionResult _OrganizersAdmin()
        {
            var _OrganizersAdmin = _context.Organizers.ToList();
            return PartialView("~/Views/OrganizersAdmin/index.cshtml", _OrganizersAdmin);
        }
      


    }
}
