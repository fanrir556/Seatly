using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using X.PagedList;

namespace Seatly1.Controllers
{
    public class SearchController : Controller
    {
        private readonly SeatlyContext _context;
        public SearchController(SeatlyContext context)
        {
            _context = context;
        }

        public class FilterData
        {
            public List<string> Hashtags { get; set; }
            public List<string> Locations { get; set; }
        }

        // 顯示search view
        public IActionResult searchIndex()
        {

            return View();
        }

        





        // 顯示右半部search partial
        public async Task<IActionResult> searchPartial(string? searchString, DateTime? searchDate)
        {
            var query = _context.NotificationRecords.AsQueryable();
            var now = DateTime.UtcNow;

            // 添加檢查 isActivity 的條件
            query = query.Where(p => p.IsActivity==true && p.EndTime> now);


            if (searchString != null)
            {
                var act = await query
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
            else if (searchDate != null)
            {
                var act = await query
                .Where(p => p.StartTime.HasValue && p.EndTime.HasValue && p.StartTime.Value.Date <= searchDate.Value.Date && p.EndTime.Value.Date >= searchDate.Value.Date)
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
            // 如果沒有符合的條件，返回 NotFound 或其他適當的值
            return NotFound();
        }

        // 顯示左半部篩選partial
        [HttpPost]
        public IActionResult sideFilterPartial([FromBody] FilterData filterData)
        {
            // 從初始搜索結果中提取所有標籤

            // 從requestData中提取hashtags和locations
            List<string> hashtags = filterData.Hashtags;
            List<string> locations = filterData.Locations;
            //var notificationRecords = _context.NotificationRecords.ToList();

            //// 提取所有哈希标签值
            var allTags = new List<string>();
            var allLocations = new List<string>();

           

            // 获取不重复的哈希标签值
            var distinctTags = hashtags.Distinct().ToList();
            var distinctLocations = locations.Distinct().ToList();
            Debug.WriteLine(distinctLocations);
            // 将哈希标签值传递给视图
            ViewBag.AllTags = distinctTags;
            ViewBag.AllLocations = distinctLocations;

            return PartialView("_sideFilterPartial");
        }

        // 篩選結果判斷
        [HttpPost]
        public async Task<IActionResult> GetActivitiesByCategories(List<string>? categories, string? searchString, DateTime? searchDate, DateTime? startDate, DateTime? endDate)
        {

            List<string> hashtags = new List<string>();
            List<string> locations = new List<string>();

            // 分類哈希標籤和地點
            foreach (var category in categories)
            {
                // 判斷是否為地點
                if (IsLocation(category))
                {
                    locations.Add(category);
                }
                else
                {
                    hashtags.Add(category);
                }
            }
            //var query = _context.NotificationRecords.AsNoTracking().AsQueryable();
            var now = DateTime.UtcNow;
            var query = _context.NotificationRecords
                        .Where(p => p.IsActivity == true && p.EndTime > now).AsNoTracking().AsQueryable();

            //var query = _context.NotificationRecords.AsQueryable();
            
            if (searchString != null) // 從首頁輸入關鍵字進來
                {

                // 添加檢查 isActivity 的條件
                //query = query.Where(p => p.IsActivity == true && p.EndTime > now);

                if (hashtags.Count > 0 && locations.Count == 0 && startDate != null && endDate != null)
                    {
                        // 分類+區間都有選
                        var filteredActivities = await query
                        .Where(p => p.ActivityName.Contains(searchString) &&
                                    p.StartTime.HasValue && p.EndTime.HasValue &&
                                    p.StartTime.Value.Date <= startDate.Value.Date &&
                                    p.EndTime.Value.Date >= endDate.Value.Date && p.EndTime.Value.Date >= startDate.Value.Date &&
                                    hashtags.Any(c =>
                                    p.HashTag1.Contains(c) ||
                                    p.HashTag2.Contains(c) ||
                                    p.HashTag3.Contains(c) ||
                                    p.HashTag4.Contains(c) ||
                                    p.HashTag5.Contains(c)))
                                    .ToListAsync();

                        return PartialView("_searchPartial", filteredActivities);
                    }

                    else if (hashtags.Count > 0 && locations.Count == 0 && startDate == null && endDate == null)
                    {
                        // 只選了分類
                        var filteredActivities = await query
                            .Where(a => a.ActivityName.Contains(searchString) &&
                                        hashtags.Any(c =>
                                            a.HashTag1.Contains(c) ||
                                            a.HashTag2.Contains(c) ||
                                            a.HashTag3.Contains(c) ||
                                            a.HashTag4.Contains(c) ||
                                            a.HashTag5.Contains(c)))
                            .ToListAsync();

                        return PartialView("_searchPartial", filteredActivities);
                    }
                    else if (hashtags.Count == 0 && locations.Count == 0 && startDate != null && endDate != null)
                    {
                        // 只選了區間
                        var filteredActivities = await query
                               .Where(a => a.ActivityName.Contains(searchString) && a.StartTime.HasValue && a.EndTime.HasValue
                                   && a.StartTime.Value.Date <= startDate.Value.Date &&
                                        a.EndTime.Value.Date >= endDate.Value.Date && a.EndTime.Value.Date >= startDate.Value.Date)
                               .ToListAsync();

                        return PartialView("_searchPartial", filteredActivities);
                    }
                else if (hashtags.Count == 0 && locations.Count > 0 && startDate == null && endDate == null)
                {
                    // 只選了地點
                    var filteredActivities = await query
                        .Where(a => a.ActivityName.Contains(searchString) && a.Location != null && a.Location != "" && locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (hashtags.Count == 0 && locations.Count > 0 && startDate != null && endDate != null)
                {
                    // 地點+區間
                    var filteredActivities = await query
                        .Where(a => a.ActivityName.Contains(searchString) && a.StartTime.HasValue && a.EndTime.HasValue &&
                                a.StartTime.Value.Date <= startDate.Value.Date &&
                                a.EndTime.Value.Date >= endDate.Value.Date &&
                                a.EndTime.Value.Date >= startDate.Value.Date &&
                                a.Location != null && a.Location != "" &&
                                locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (hashtags.Count > 0 && locations.Count > 0 && startDate == null && endDate == null)
                {
                    // 分類+地點
                    var filteredActivities = await query
                        .Where(a => a.ActivityName.Contains(searchString) && hashtags.Any(c =>
                                a.HashTag1.Contains(c) ||
                                a.HashTag2.Contains(c) ||
                                a.HashTag3.Contains(c) ||
                                a.HashTag4.Contains(c) ||
                                a.HashTag5.Contains(c)) &&
                                a.Location != null && a.Location != "" &&
                                locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (hashtags.Count > 0 && locations.Count > 0 && startDate != null && endDate != null)
                {
                    // 分類+地區+區間
                    var filteredActivities = await query
                        .Where(a => a.ActivityName.Contains(searchString) && a.StartTime.HasValue && a.EndTime.HasValue &&
                                a.StartTime.Value.Date <= startDate.Value.Date &&
                                a.EndTime.Value.Date >= endDate.Value.Date &&
                                a.EndTime.Value.Date >= startDate.Value.Date &&
                                hashtags.Any(c =>
                                a.HashTag1.Contains(c) ||
                                a.HashTag2.Contains(c) ||
                                a.HashTag3.Contains(c) ||
                                a.HashTag4.Contains(c) ||
                                a.HashTag5.Contains(c)) &&
                                a.Location != null && a.Location != "" &&
                                locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else
                    {
                        // 原始搜尋結果
                        var act = await query   
                            .Where(p => p.ActivityName.Contains(searchString))
                            .ToListAsync();

                        return PartialView("_searchPartial", act);
                    }
                }
                else if (searchDate != null) // 從首頁輸入指定日期進來
                {
                    if (hashtags.Count > 0 && startDate != null && endDate != null)
                    {
                        // 分類+區間都有選
                        var filteredActivities = await query        
                            .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                                   && a.StartTime.Value.Date <= startDate.Value.Date &&
                                      a.EndTime.Value.Date >= endDate.Value.Date && a.EndTime.Value.Date >= startDate.Value.Date
                                && hashtags.Any(c =>
                                    a.HashTag1.Contains(c) ||
                                    a.HashTag2.Contains(c) ||
                                    a.HashTag3.Contains(c) ||
                                    a.HashTag4.Contains(c) ||
                                    a.HashTag5.Contains(c) &&
                                    a.StartTime.Value.Date <= searchDate.Value.Date &&
                                    a.EndTime.Value.Date >= searchDate.Value.Date))
                                    .ToListAsync();

                        return PartialView("_searchPartial", filteredActivities);
                    }
                    else if (hashtags.Count > 0 && startDate == null && endDate == null)
                    {
                        // 只選了分類
                        var filteredActivities = await query
                            .Where(a => a.StartTime.HasValue && a.EndTime.HasValue && a.StartTime.Value.Date <= searchDate.Value.Date && a.EndTime.Value.Date >= searchDate.Value.Date && hashtags.Any(c =>
                                    a.HashTag1.Contains(c) ||
                                    a.HashTag2.Contains(c) ||
                                    a.HashTag3.Contains(c) ||
                                    a.HashTag4.Contains(c) ||
                                    a.HashTag5.Contains(c)))
                            .ToListAsync();

                        return PartialView("_searchPartial", filteredActivities);
                    }
                    else if (hashtags.Count == 0 && startDate != null && endDate != null)
                    {
                        // 只選了區間
                        var filteredActivities = await query
                            .Where(a => a.StartTime.HasValue && a.EndTime.HasValue && a.StartTime.Value.Date <= searchDate.Value.Date && a.EndTime.Value.Date >= searchDate.Value.Date
                                   && a.StartTime.Value.Date <= startDate.Value.Date &&
                                        a.EndTime.Value.Date >= endDate.Value.Date && a.EndTime.Value.Date >= startDate.Value.Date)
                            .ToListAsync();

                        return PartialView("_searchPartial", filteredActivities);
                    }
                else if (hashtags.Count == 0 && locations.Count > 0 && startDate == null && endDate == null)
                {
                    // 只選了地點
                    var filteredActivities = await query
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                                && a.StartTime.Value.Date <= searchDate.Value.Date
                                && a.EndTime.Value.Date >= searchDate.Value.Date && a.Location != null && a.Location != "" && locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (hashtags.Count == 0 && locations.Count > 0 && startDate != null && endDate != null)
                {
                    // 地點+區間
                    var filteredActivities = await query
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                                && a.StartTime.Value.Date <= searchDate.Value.Date
                                && a.EndTime.Value.Date >= searchDate.Value.Date && a.StartTime.HasValue && a.EndTime.HasValue &&
                                a.StartTime.Value.Date <= startDate.Value.Date &&
                                a.EndTime.Value.Date >= endDate.Value.Date &&
                                a.EndTime.Value.Date >= startDate.Value.Date &&
                                a.Location != null && a.Location != "" &&
                                locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (hashtags.Count > 0 && locations.Count > 0 && startDate == null && endDate == null)
                {
                    // 分類+地點
                    var filteredActivities = await query
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                                && a.StartTime.Value.Date <= searchDate.Value.Date
                                && a.EndTime.Value.Date >= searchDate.Value.Date && hashtags.Any(c =>
                                a.HashTag1.Contains(c) ||
                                a.HashTag2.Contains(c) ||
                                a.HashTag3.Contains(c) ||
                                a.HashTag4.Contains(c) ||
                                a.HashTag5.Contains(c)) &&
                                a.Location != null && a.Location != "" &&
                                locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (hashtags.Count > 0 && locations.Count > 0 && startDate != null && endDate != null)
                {
                    // 分類+地區+區間
                    var filteredActivities = await query
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                                && a.StartTime.Value.Date <= searchDate.Value.Date
                                && a.EndTime.Value.Date >= searchDate.Value.Date && a.StartTime.HasValue && a.EndTime.HasValue &&
                                a.StartTime.Value.Date <= startDate.Value.Date &&
                                a.EndTime.Value.Date >= endDate.Value.Date &&
                                a.EndTime.Value.Date >= startDate.Value.Date &&
                                hashtags.Any(c =>
                                a.HashTag1.Contains(c) ||
                                a.HashTag2.Contains(c) ||
                                a.HashTag3.Contains(c) ||
                                a.HashTag4.Contains(c) ||
                                a.HashTag5.Contains(c)) &&
                                a.Location != null && a.Location != "" &&
                                locations.Any(l =>
                                a.Location.Contains(l)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else
                    {
                        // 原始搜尋結果
                        var act = await query
                            .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                                && a.StartTime.Value.Date <= searchDate.Value.Date
                                && a.EndTime.Value.Date >= searchDate.Value.Date)
                            .ToListAsync();

                        return PartialView("_searchPartial", act);
                    }
                }
                return NotFound();
            }
        //    return NotFound();
        //}

        // 判斷是否為地點
        private bool IsLocation(string category)
        {
            // 假設地點都是台北、台中、高雄、台南等城市名稱
            // 並且哈希標籤不包含 '#' 符號

            string[] cities = { "台北", "台中", "高雄", "台南" };
            foreach (var city in cities)
            {
                if (category.StartsWith(city))
                {
                    return true;
                }
            }
            return false;
        }


      

    }
}
