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

        //GET:Admin/_NotificationRecord
        public IActionResult _NotificationRecord()
        {
            var notificationRecords = _context.NotificationRecords.ToList();
            return PartialView("~/Views/Admin/_NotificationRecord-Index.cshtml", notificationRecords);
        }


        //POST:Admin/_NotificationRecord_Delete
        [HttpPost]
        public async Task<IActionResult> _NotificationRecord_Delete(int id) {
            var notificationRecord = await _context.NotificationRecords.FindAsync(id);
            if (notificationRecord != null)
            {
                _context.NotificationRecords.Remove(notificationRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(_NotificationRecord));
        }


        public IActionResult _AspNetUsers()
        {
            var _AspNetUsers = _context.AspNetUsers.ToList();
            return PartialView("~/Views/Admin/_AspNetUsers-Index.cshtml", _AspNetUsers);
        }

        //POST:Admin/_AspNetUsers_Delete
        [HttpPost]
        public async Task _AspNetUsers_Delete(string id)
        {
            var ANU = await _context.AspNetUsers.FirstOrDefaultAsync(a=>a.Id== id);
            if (ANU != null)
            {
                _context.AspNetUsers.Remove(ANU);
            }

            await _context.SaveChangesAsync();
        }

        public IActionResult _OrganizersAdmin()
        {
            var _OrganizersAdmin = _context.Organizers.ToList();
            return PartialView("~/Views/Admin/_OrganizersAdmin-Index.cshtml", _OrganizersAdmin);
        }

        //POST:Admin/_AspNetUsers_Delete
        [HttpPost]
        public async Task _OrganizersAdmin_Delete(int id)
        {
            var O1 = await _context.Organizers.FirstOrDefaultAsync(a => a.OrganizerId == id);
            if (O1 != null)
            {
                _context.Organizers.Remove(O1);
            }

            await _context.SaveChangesAsync();
        }




        //POST:Admin/_NotificationRecord_Activty
        [HttpPost]
        public async Task<IEnumerable<NotificationRecord>> _find_Activty(NotificationRecord NTB)
        {
            return _context.NotificationRecords.Where(e => e.OrganizerId == NTB.OrganizerId).ToList();
        }

        //輸出Partial頁面
        public IActionResult _Check()
        { 
            return PartialView();
        }

    }
}
