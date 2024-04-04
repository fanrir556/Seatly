using Microsoft.AspNetCore.Mvc;
using Seatly1.DTO;
using Seatly1.Models;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Seatly1.Controllers
{
    // 活動方登入註冊API：api/Organizer
    public class OrganizerController : Controller
    {
        SeatlyContext _context;
        public OrganizerController(SeatlyContext context)
            {
            _context = context;
            }

        // 活動方登入api
        //[HttpPost]
        //public ActionResult Login(FormCollection post)
        //{
        //    string account = post["account"];
        //    string password = post["password"];

        //    //驗證密碼
        //    if (db.CheckUserData(account, password))
        //    {
        //        Response.Redirect("~/Home/Home");
        //        return new EmptyResult();
        //    }
        //    else
        //    {
        //        ViewBag.Msg = "登入失敗...";
        //        return View();
        //    }
        //}

        // 新增活動方api
        [HttpPost]
        public async Task<IActionResult> PostOrganizer(OrganizerDTO Organizer)
        {
            if (!ModelState.IsValid)
            {
                // 返回驗證錯誤
                return BadRequest(ModelState);
            }

            // Validate email format
            if (!Organizer.Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "Email must contain '@' character.");
                return BadRequest(ModelState);
            }

            Organizer emp = new Organizer
            {
                OrganizerAccount = Organizer.OrganizerAccount,
                LoginPassword = Organizer.LoginPassword,
                OrganizerName = Organizer.OrganizerName,
                OrganizerCategory = Organizer.OrganizerCategory,
                OrganizerPhoto = Organizer.OrganizerPhoto,
                Menu = Organizer.Menu,
                Address = Organizer.Address,
                ReservationUrl = Organizer.ReservationUrl,
                Hashtag = Organizer.Hashtag,
                Email = Organizer.Email,
                Phone = Organizer.Phone,
                Validation = Organizer.Validation,
            };

            // Save to database (replace with your actual database logic)
            // _context.Organizers.Add(emp);
            // await _context.SaveChangesAsync();

            return Ok($"新增成功活動方編號:{emp.OrganizerId}");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
