using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Seatly1.Data;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        SeatlyContext _context;
        UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,SeatlyContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            /*簽到判定*/
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                if (aspUser == null)
                {
                    return NotFound();
                }
                else
                {
                    DateOnly date = DateOnly.FromDateTime(DateTime.Now.Date);
                    var dCheckIn = await _context.DailyCheckIns.FirstOrDefaultAsync(s => s.MemberId == aspUser.Id && s.CheckInTime == date);
                    var gameCountList = await _context.GamePoints.Where(s => s.MemberId == aspUser.Id && s.PointsDate == date).ToListAsync();
                    int gameCount = gameCountList.Count;
                    if (dCheckIn == null)
                    {
                        HttpContext.Session.SetString("checkedIn", "false");
                    }
                    else
                    {
                        HttpContext.Session.Remove("checkedIn");
                    }
                    if (gameCount < 3)
                    {
                        HttpContext.Session.SetString("gamed", "false");
                    }
                    else
                    {
                        HttpContext.Session.Remove("gamed");
                    }
                }
            }
            else
            {
                HttpContext.Session.Remove("checkedIn");
                HttpContext.Session.Remove("gamed");
            }
            /*簽到判定*/

            // 首頁熱門選項
            var query = _context.NotificationRecords.AsQueryable();
            var now = DateTime.UtcNow;

            // 添加檢查 isActivity 的條件
            query = query.Where(p => p.IsActivity == true && p.EndTime > now);

            var hotItems = await query
                .Where(r => r.HashTag1.Contains("HOT") ||
                r.HashTag2.Contains("HOT") ||
                r.HashTag3.Contains("HOT") ||
                r.HashTag4.Contains("HOT") ||
                r.HashTag5.Contains("HOT"))
                 .OrderBy(r => Guid.NewGuid()) // 隨機排序
                 .Take(10) // 選取10筆
                 .ToListAsync();
            Debug.WriteLine("熱門:" + hotItems.Count);

            return View(hotItems);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //mainModal
        public IActionResult mainModal()
        {
            return PartialView("_mainModalPartial");
        }
        //mainModal

        //管理員Session設定
        [HttpPost]
        public IActionResult isMG(string key)
        {
            if (key == "true")
            {
                HttpContext.Session.SetString("isMg", key);
            }
            else
            {
                HttpContext.Session.Remove("isMg");
            }
            return Ok();
        }

        
    }
}
