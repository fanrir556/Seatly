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

        ////顯示右半部search partial，加分頁
        //public async Task<IActionResult> searchPartial(string? searchString, DateTime? searchDate, int? page)
        //{
        //    Debug.WriteLine($"Received searchString: {searchString}, searchDate: {searchDate}, page: {page}");


        //    IQueryable<NotificationRecord> query = _context.NotificationRecords;

        //    if (searchString != null)
        //    {
        //        query = query.Where(p => p.ActivityName.Contains(searchString));
        //        // 每頁顯示的數據量
        //        int pageSize = 5;
        //        // 計算當前頁數
        //        int pageNumber = (page ?? 1);

        //        // 將查詢結果分頁化
        //        var pagedData = await query.OrderByDescending(p => p.ActivityId)
        //                                   .ToPagedListAsync(pageNumber, pageSize);

        //        return PartialView("_searchPartial", pagedData);
        //    }
        //    else if (searchDate != null)
        //    {
        //        query = query.Where(p => p.StartTime.HasValue && p.EndTime.HasValue &&
        //  p.StartTime.Value.Date <= searchDate.Value.Date && p.EndTime.Value.Date >= searchDate.Value.Date);
        //        // 每頁顯示的數據量
        //        int pageSize = 5;
        //        // 計算當前頁數
        //        int pageNumber = (page ?? 1);

        //        // 將查詢結果分頁化
        //        var pagedData = await query.OrderByDescending(p => p.ActivityId)
        //                                   .ToPagedListAsync(pageNumber, pageSize);

        //        return PartialView("_searchPartial", pagedData);
        //    }
        //    // 如果沒有符合的條件，返回 NotFound 或其他適當的值
        //    return NotFound();
        //}





        // 顯示右半部search partial
        public async Task<IActionResult> searchPartial(string? searchString, DateTime? searchDate)
        {


            if (searchString != null)
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
            else if (searchDate != null)
            {
                var act = await _context.NotificationRecords
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
            // 待修!! 應該要從searchPartial裡抓搜尋結果的標籤

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
            if (searchString != null) // 從首頁輸入關鍵字進來
            {
                if (categories.Count > 0 && startDate != null && endDate != null)
                {
                    // 分類+區間都有選
                    var filteredActivities = await _context.NotificationRecords
                    .Where(p => p.ActivityName.Contains(searchString) &&
                                p.StartTime.HasValue && p.EndTime.HasValue &&
                                p.StartTime.Value.Date <= startDate.Value.Date &&
                                p.EndTime.Value.Date >= endDate.Value.Date && p.EndTime.Value.Date >= startDate.Value.Date &&
                                categories.Any(c =>
                                p.HashTag1.Contains(c) ||
                                p.HashTag2.Contains(c) ||
                                p.HashTag3.Contains(c) ||
                                p.HashTag4.Contains(c) ||
                                p.HashTag5.Contains(c)))
                                .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }

                else if (categories.Count > 0 && startDate == null && endDate == null)
                {
                    // 只選了分類
                    var filteredActivities = await _context.NotificationRecords
                        .Where(a => a.ActivityName.Contains(searchString) &&
                                    categories.Any(c =>
                                        a.HashTag1.Contains(c) ||
                                        a.HashTag2.Contains(c) ||
                                        a.HashTag3.Contains(c) ||
                                        a.HashTag4.Contains(c) ||
                                        a.HashTag5.Contains(c)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (categories.Count == 0 && startDate != null && endDate != null)
                {
                    // 只選了區間
                    var filteredActivities = await _context.NotificationRecords
                           .Where(a => a.ActivityName.Contains(searchString) && a.StartTime.HasValue && a.EndTime.HasValue
                               && a.StartTime.Value.Date <= startDate.Value.Date &&
                                    a.EndTime.Value.Date >= endDate.Value.Date && a.EndTime.Value.Date >= startDate.Value.Date)
                           .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else
                {
                    // 原始搜尋結果
                    var act = await _context.NotificationRecords
                        .Where(p => p.ActivityName.Contains(searchString))
                        .ToListAsync();

                    return PartialView("_searchPartial", act);
                }
            }
            else if (searchDate != null) // 從首頁輸入指定日期進來
            {
                if (categories.Count > 0 && startDate != null && endDate != null)
                {
                    // 分類+區間都有選
                    var filteredActivities = await _context.NotificationRecords
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                               && a.StartTime.Value.Date <= startDate.Value.Date &&
                                  a.EndTime.Value.Date >= endDate.Value.Date && a.EndTime.Value.Date >= startDate.Value.Date
                            && categories.Any(c =>
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
                else if (categories.Count > 0 && startDate == null && endDate == null)
                {
                    // 只選了分類
                    var filteredActivities = await _context.NotificationRecords
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue && a.StartTime.Value.Date <= searchDate.Value.Date && a.EndTime.Value.Date >= searchDate.Value.Date && categories.Any(c =>
                                a.HashTag1.Contains(c) ||
                                a.HashTag2.Contains(c) ||
                                a.HashTag3.Contains(c) ||
                                a.HashTag4.Contains(c) ||
                                a.HashTag5.Contains(c)))
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else if (categories.Count == 0 && startDate != null && endDate != null)
                {
                    // 只選了區間
                    var filteredActivities = await _context.NotificationRecords
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue && a.StartTime.Value.Date <= searchDate.Value.Date && a.EndTime.Value.Date >= searchDate.Value.Date
                               && a.StartTime.Value.Date <= startDate.Value.Date &&
                                    a.EndTime.Value.Date >= endDate.Value.Date && a.EndTime.Value.Date >= startDate.Value.Date)
                        .ToListAsync();

                    return PartialView("_searchPartial", filteredActivities);
                }
                else
                {
                    // 原始搜尋結果
                    var act = await _context.NotificationRecords
                        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
                            && a.StartTime.Value.Date <= searchDate.Value.Date
                            && a.EndTime.Value.Date >= searchDate.Value.Date)
                        .ToListAsync();

                    return PartialView("_searchPartial", act);
                }
            }
            return NotFound();
        }




        //[HttpPost]
        //public IActionResult GetActivitiesByCategories(List<string>? categories, string? searchString, DateTime? searchDate)
        //{
        //    if (searchString != null)
        //    {
        //        if (categories == null)
        //        {
        //            var act = _context.NotificationRecords
        //               .Where(p => p.ActivityName.Contains(searchString))
        //               .ToListAsync();

        //            return PartialView("_searchPartial", act);
        //        }
        //        else
        //        {
        //            var filteredActivities = _context.NotificationRecords
        //               .Where(a => a.ActivityName.Contains(searchString) &&
        //                           categories.Any(c =>
        //                               a.HashTag1.Contains(c) ||
        //                               a.HashTag2.Contains(c) ||
        //                               a.HashTag3.Contains(c) ||
        //                               a.HashTag4.Contains(c) ||
        //                               a.HashTag5.Contains(c)));

        //            return PartialView("_searchPartial", filteredActivities);

        //        }
        //    }
        //    else if (searchDate != null)
        //    {
        //        if (categories == null)
        //        {
        //            var act = _context.NotificationRecords
        //               .Where(p => p.StartTime.HasValue && p.EndTime.HasValue && p.StartTime.Value.Date <= searchDate.Value.Date && p.EndTime.Value.Date >= searchDate.Value.Date)
        //               .ToListAsync();

        //            return PartialView("_searchPartial", act);
        //        }
        //        else
        //        {
        //            var filteredActivities = _context.NotificationRecords
        //               .Where(a => a.StartTime.HasValue && a.EndTime.HasValue && a.StartTime.Value.Date <= searchDate.Value.Date && a.EndTime.Value.Date >= searchDate.Value.Date &&
        //                           categories.Any(c =>
        //                               a.HashTag1.Contains(c) ||
        //                               a.HashTag2.Contains(c) ||
        //                               a.HashTag3.Contains(c) ||
        //                               a.HashTag4.Contains(c) ||
        //                               a.HashTag5.Contains(c)));

        //            return PartialView("_searchPartial", filteredActivities);

        //        }
        //    }
        //    return NotFound();
        //}


       

        //[HttpPost]
        //public IActionResult SearchByDate(string searchDate)
        //{
        //    if (DateTime.TryParseExact(searchDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        //    {
        //        var activities = _context.NotificationRecords.Where(a => a.StartTime == parsedDate).ToList();
        //        return PartialView("_searchPartial", activities);
        //    }
        //    else
        //    {
        //        // 處理日期格式無效的情況
        //        return BadRequest("Invalid date format");
        //    }
        //}

    }
}
