using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;
using System.Diagnostics;
using System.Globalization;

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

        public async Task<IActionResult> searchPartial(string? searchString,DateTime? searchDate)
        {
            if(searchString != null) { 
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
            }else if(searchDate != null) 
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

        [HttpPost]
        public IActionResult sideFilterPartial([FromBody]List<string> hashtags)
        {
            // 從初始搜索結果中提取所有標籤
            // 待修!! 應該要從searchPartial裡抓搜尋結果的標籤

            //var notificationRecords = _context.NotificationRecords.ToList();

            //// 提取所有哈希标签值
            var allTags = new List<string>();

            //foreach (var record in hashtags)
            //{
            //    // 获取每个活动记录的哈希标签字段的值
            //    for (int i = 1; i <= 5; i++)
            //    {
            //        // 构建哈希标签字段的名称
            //        string tagName = $"HashTag{i}";

            //        // 获取当前活动记录的哈希标签字段的值
            //        var tagValue = record.GetType().GetProperty(tagName).GetValue(record, null) as string;

            //        // 如果哈希标签字段的值不为空，则添加到列表中
            //        if (!string.IsNullOrEmpty(tagValue))
            //        {
            //            // 将当前哈希标签字段的值添加到列表中
            //            allTags.Add(tagValue);
            //        }
            //    }
            //}

            // 获取不重复的哈希标签值
            var distinctTags = hashtags.Distinct().ToList();

            // 将哈希标签值传递给视图
            ViewBag.AllTags = distinctTags;

            return PartialView("_sideFilterPartial");
        }


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


        //public IActionResult GetActivitiesByDateRange(DateTime startDate, DateTime endDate)
        //{
        //    var filteredActivities = _context.NotificationRecords
        //        .Where(a => a.StartTime.HasValue && a.EndTime.HasValue
        //            && a.StartTime.Value.Date >= startDate.Date
        //            && a.EndTime.Value.Date <= endDate.Date)
        //        .ToListAsync();

        //    return PartialView("_searchPartial", filteredActivities);
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
