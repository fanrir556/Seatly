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
        public async Task<IActionResult> GetActivitiesByLocation (string? location,string? selectedType)
        {
            if (string.IsNullOrEmpty(location))
            {
                return BadRequest("Location parameter is required.");
            }
            var query = _context.NotificationRecords.AsQueryable();
            var now = DateTime.UtcNow;

            // 添加檢查 isActivity 的條件
            query = query.Where(p => p.IsActivity == true && p.EndTime > now);

            var activities = await query
        .Where(a => a.Location == location && (
                a.HashTag1.Contains(selectedType) ||
                a.HashTag2.Contains(selectedType) ||
                a.HashTag3.Contains(selectedType) ||
                a.HashTag4.Contains(selectedType) ||
                a.HashTag5.Contains(selectedType)
            ))
        .ToListAsync();

            return Json(activities);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetActivitiesByHashtag(string? selectedType)
        //{
        //    if (string.IsNullOrEmpty(selectedType))
        //    {
        //        return BadRequest("SelectedType parameter is required.");
        //    }

        //    // 根據 selectedType 找到符合條件的活動資料
        //    var activities = await _context.NotificationRecords
        //        .Where(a => a.HashTag1.Contains(selectedType) ||
        //                    a.HashTag2.Contains(selectedType) ||
        //                    a.HashTag3.Contains(selectedType) ||
        //                    a.HashTag4.Contains(selectedType) ||
        //                    a.HashTag5.Contains(selectedType))
        //        .ToListAsync();

        //    return Json(activities);
        //}
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
