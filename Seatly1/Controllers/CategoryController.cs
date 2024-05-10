using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;
using System.Linq;

namespace Seatly1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly SeatlyContext _context;

        public CategoryController(SeatlyContext context)
        {
            _context = context;
        }

        public IActionResult categoryIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetActivitiesByHashtag (string? hashtag)
        {
            var activities = await _context.NotificationRecords.Where(a => 
                                a.HashTag1.Contains(hashtag) || 
                                a.HashTag2.Contains(hashtag) ||
                                a.HashTag3.Contains(hashtag) ||
                                a.HashTag4.Contains(hashtag) ||
                                a.HashTag5.Contains(hashtag)).ToListAsync();
            return Json(activities);
        }
    }
}
