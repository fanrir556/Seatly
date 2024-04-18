using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class SearchController : Controller
    {
        private readonly SeatlyContext _context;
        public SearchController(SeatlyContext context)
        {
            _context = context;
        }

        public IActionResult searchIndex()
        {

            return View();
        }

        public async Task<IActionResult> searchPartial(string searchString)
        {

            var act = await _context.NotificationRecords
                .Where(p => p.ActivityName.Contains(searchString))
                .ToListAsync();

            if (act != null)
            {
                return PartialView("_searchPartial", act);
            }
            else
            {
                return NotFound();
            }

        }

        public IActionResult sideFilterPartial()
        {
            // 從初始搜索結果中提取所有標籤
            // 待修!! 應該要從searchPartial裡抓搜尋結果的標籤
            var notificationRecords = _context.NotificationRecords.ToList();
            var allTags = new List<string>();

            foreach (var record in notificationRecords)
            {
                var tags = record.HashTag.Split(',', StringSplitOptions.RemoveEmptyEntries);
                allTags.AddRange(tags);
            }

            var distinctTags = allTags.Distinct().ToList();
            ViewBag.AllTags = distinctTags;


            return PartialView("_sideFilterPartial");
        }


        [HttpPost]
        public IActionResult GetActivitiesByCategories(List<string> categories, string searchString)
        {


            var filteredActivities = _context.NotificationRecords
                .Where(a => a.ActivityName.Contains(searchString) || categories.Any(c => a.HashTag.Contains(c)))
                .ToList();





            //// 符合搜尋字串的值
            //var allActivities = _context.NotificationRecords
            //    .Where(a => a.ActivityName.Contains(searchString))
            //    .ToList();

            ////// 符合搜尋字串中, 符合任一hashtag的值
            //var filteredActivities = allActivities
            //    .Where(a => categories.Any(c => a.HashTag.Split(',', StringSplitOptions.RemoveEmptyEntries).Contains(c)))
            //    .ToList();


            return PartialView("_searchPartial", filteredActivities);


        }
    }
}
