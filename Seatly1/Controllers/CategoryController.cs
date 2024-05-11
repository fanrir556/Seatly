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

        //[HttpGet]
        //public async Task<IActionResult> GetActivitiesByTypeAndLocation(string type, string location)
        //{
        //    try
        //    {
        //        var query = _context.NotificationRecords.AsQueryable();

        //        if (!string.IsNullOrEmpty(type))
        //        {
        //            query = query.Where(a =>
        //                a.HashTag1.Contains(type) || a.HashTag2.Contains(type) || a.HashTag3.Contains(type) || a.HashTag4.Contains(type) || a.HashTag5.Contains(type));
        //        }

        //        if (!string.IsNullOrEmpty(location))
        //        {
        //            query = query.Where(a => a.Location.Contains(location));
        //        }

        //        var filteredActivities = await query.ToListAsync();

        //        return Json(filteredActivities);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


    }
}
