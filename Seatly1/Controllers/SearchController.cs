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
        private DateTime parsedDate;

        public SearchController(SeatlyContext context)
        {
            _context = context;
        }

        public IActionResult searchIndex()
        {

            return View();
        }

        // 產生搜尋結果畫面
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

        public class FilterData
        {
            public List<string> Hashtags { get; set; }
            public List<string> StartDates { get; set; }

            public List<string> EndDates { get; set; }
        }

        // 產生篩選標籤
        [HttpPost]
        public IActionResult SideFilterPartial([FromBody] FilterData filterData)
        {
            if (filterData != null)
            {
                var distinctTags = filterData.Hashtags.Distinct().ToList();
                var distinctStartDates = filterData.StartDates.Distinct().ToList();
                var distinctEndDates = filterData.EndDates.Distinct().ToList();

                ViewBag.AllTags = distinctTags;
                ViewBag.AllStartDates = distinctStartDates;
                ViewBag.AllEndDates = distinctEndDates;

                return PartialView("_sideFilterPartial");
            }

            return BadRequest("Invalid filter data");
        }


        //[HttpPost]
        //public IActionResult sideFilterPartial([FromBody]List<string> hashtags)
        //{
        //    // 從初始搜索結果中提取所有標籤
        //    // 待修!! 應該要從searchPartial裡抓搜尋結果的標籤

        //    //var notificationRecords = _context.NotificationRecords.ToList();

        //    //// 提取所有哈希标签值
        //    var allTags = new List<string>();

        //    //foreach (var record in hashtags)
        //    //{
        //    //    // 获取每个活动记录的哈希标签字段的值
        //    //    for (int i = 1; i <= 5; i++)
        //    //    {
        //    //        // 构建哈希标签字段的名称
        //    //        string tagName = $"HashTag{i}";

        //    //        // 获取当前活动记录的哈希标签字段的值
        //    //        var tagValue = record.GetType().GetProperty(tagName).GetValue(record, null) as string;

        //    //        // 如果哈希标签字段的值不为空，则添加到列表中
        //    //        if (!string.IsNullOrEmpty(tagValue))
        //    //        {
        //    //            // 将当前哈希标签字段的值添加到列表中
        //    //            allTags.Add(tagValue);
        //    //        }
        //    //    }
        //    //}

        //    // 获取不重复的哈希标签值
        //    var distinctTags = hashtags.Distinct().ToList();
        //    //var distinctStartDates = startdates.Distinct().ToList();

        //    // 将哈希标签值传递给视图
        //    ViewBag.AllTags = distinctTags;
        //    //ViewBag.AllStartDates = distinctStartDates;

        //    return PartialView("_sideFilterPartial");
        //}


        // 分類checkbox判斷
        [HttpPost]
        public IActionResult GetActivitiesByCategories(List<string>? categories, string? searchString, DateTime? searchDate)
        {
            if (searchString != null)
            {
                if (categories == null)
                {
                    var act = _context.NotificationRecords
                       .Where(p => p.ActivityName.Contains(searchString))
                       .ToListAsync();

                    return PartialView("_searchPartial", act);
                }
                else
                {
                    // 分离日期格式和普通字符串
                    List<string> normalStrings = new List<string>();
                    List<DateTime> dateStrings = new List<DateTime>();

                    foreach (var category in categories)
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(category, "yyyy/M/d", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ||
                    DateTime.TryParse(category, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            dateStrings.Add(date);
                        }
                        else
                        {
                            normalStrings.Add(category);
                        }
                    }

                    // 根据需要执行相应的逻辑
                    var filteredActivities = _context.NotificationRecords
                       .Where(a => a.ActivityName.Contains(searchString) &&
                            (normalStrings.Any(c =>
                                a.HashTag1.Contains(c) ||
                                a.HashTag2.Contains(c) ||
                                a.HashTag3.Contains(c) ||
                                a.HashTag4.Contains(c) ||
                                a.HashTag5.Contains(c)) ||
                            dateStrings.Any(d =>
                                a.StartTime.HasValue && a.StartTime.Value.Date == d.Date)));

                    return PartialView("_searchPartial", filteredActivities);

                }
            }
            else if (searchDate != null)
            {
                if (categories == null)
                {
                    var act = _context.NotificationRecords
                       .Where(p => p.StartTime.HasValue && p.EndTime.HasValue && p.StartTime.Value.Date <= searchDate.Value.Date && p.EndTime.Value.Date >= searchDate.Value.Date)
                       .ToListAsync();

                    return PartialView("_searchPartial", act);
                }
                else
                {
                    // 分离日期格式和普通字符串
                    List<string> normalStrings = new List<string>();
                    List<DateTime> dateStrings = new List<DateTime>();

                    foreach (var category in categories)
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(category, "yyyy/M/d", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ||
                    DateTime.TryParse(category, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            dateStrings.Add(date);
                        }
                        else
                        {
                            normalStrings.Add(category);
                        }
                    }

                    var filteredActivities = _context.NotificationRecords
                                            .Where(a => a.StartTime.HasValue && a.EndTime.HasValue 
                                            && a.StartTime.Value.Date <= searchDate.Value.Date 
                                            && a.EndTime.Value.Date >= searchDate.Value.Date &&
                                            (normalStrings.Any(c =>
                                                a.HashTag1.Contains(c) ||
                                                a.HashTag2.Contains(c) ||
                                                a.HashTag3.Contains(c) ||
                                                a.HashTag4.Contains(c) ||
                                                a.HashTag5.Contains(c)) ||
                                             dateStrings.Any(d =>
                                                a.StartTime.HasValue && a.StartTime.Value.Date == d.Date)));

                    return PartialView("_searchPartial", filteredActivities);

                }
            }
            return NotFound();
        }


        

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
