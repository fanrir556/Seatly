using Microsoft.AspNetCore.Mvc;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    public class FriendsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return RedirectToPage("/Areas/Identity/Pages/Account/Manage/Friends.cshtml"); // 重定向到 Razor 页面
        //}

        private readonly SeatlyContext _context;

        public FriendsController(SeatlyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var friends = new List<string> { "Friend 1", "Friend 2", "Friend 3" };
            ViewBag.FriendsList = friends;
            return RedirectToPage("/Areas/Identity/Pages/Account/Manage/Friends.cshtml");

        }


    }
}
