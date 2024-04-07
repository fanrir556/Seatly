using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class OrganizersController : Controller
    {
        private readonly SeatlyContext _context;

        public OrganizersController(SeatlyContext context)
        {
            _context = context;
        }

        public IActionResult Search(string searchString)
        {
            Debug.WriteLine("Search String: " + searchString); // 在控制台输出搜索字符串

            var organizer = _context.Organizers
                .Where(p => p.OrganizerName.Contains(searchString))
                .ToList();

            Debug.WriteLine("Organizers Count: " + organizer.Count); // 在控制台输出查询到的组织者数量
            //var query = _context.Organizers.Where(o => o.OrganizerName.Contains("searchString"));
            //Console.WriteLine(query.ToQueryString());
            return View(organizer);
        }
    }
}
