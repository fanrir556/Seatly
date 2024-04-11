using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Search(string searchString)
        {
            Debug.WriteLine("Search String: " + searchString); // 在控制台输出搜索字符串

            var act = _context.NotificationRecords
                .Where(p => p.ActivityName.Contains(searchString))
                .ToList();

            Debug.WriteLine("Activity Count: " + act.Count); // 在控制台输出查询到的组织者数量
            //var query = _context.Organizers.Where(o => o.OrganizerName.Contains("searchString"));
            //Console.WriteLine(query.ToQueryString());
            return View(act);
        }
    }
}
