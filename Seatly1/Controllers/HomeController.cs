using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Seatly1.Data;
using Seatly1.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Seatly1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        SeatlyContext _context;
        UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SeatlyContext context, UserManager<ApplicationUser> userManager)
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
                    var gameCountList = await _context.GamePoints.Where(s => s.MemberId == aspUser.Id && s.PointsDate == date && s.GameType == 1).ToListAsync();
                    var logoGameCountList = await _context.GamePoints.Where(s => s.MemberId == aspUser.Id && s.PointsDate == date && s.GameType == 2).ToListAsync();
                    int gameCount = gameCountList.Count;
                    int logoGameCount = logoGameCountList.Count;
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
                    if (logoGameCount < 1)
                    {
                        HttpContext.Session.SetString("logoed", "false");
                    }
                    else
                    {
                        HttpContext.Session.Remove("logoed");
                    }
                }
            }
            else
            {
                HttpContext.Session.Remove("checkedIn");
                HttpContext.Session.Remove("gamed");
                HttpContext.Session.Remove("logoed");
            }
            /*簽到判定*/

            // 首頁熱門選項
            var query = _context.NotificationRecords.AsQueryable();
            var now = DateTime.UtcNow;

            // 添加檢查 isActivity 的條件
            query = query.Where(p => p.IsActivity == true && p.EndTime > now);

            var hotItems = await query
                .Where(r => 
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

        public IActionResult CheckIsMG()
        {
            string isMG = HttpContext.Session.GetString("isMg");
            if (!string.IsNullOrEmpty(isMG))
            {
                return Content(isMG, "text/plain");
            }
            return Content("false", "text/plain");
        }

        public IActionResult MGLogin([FromBody] string pwd)
        {
            string mgJS = @"
                sessionStorage.setItem('isManager', 'true');

                var form = new FormData();
                form.append('key', 'true');
                fetch(`" + Url.Action("isMG", "Home") + @"`, {
                    method: 'POST',
                    body: form,
                }).then(function (response) {
                    window.location.href = '/';
                }).catch(function (err) {
                    alert(err);
                });";
            string mgName = "";
            string mgImg = "";

            switch (pwd)
            {
                case "5487":
                    var result = new
                    {
                        mgName = "",
                        mgImg = "",
                        mgJS = mgJS
                    };
                    return Content(JsonConvert.SerializeObject(result), "application/json");
                case "54T70":
                    mgName = "T70";
                    mgImg = Url.Content("~/images/T70.png");
                    var resultT70 = new
                    {
                        mgName = mgName,
                        mgImg = mgImg,
                        mgJS = mgJS
                    };
                    return Content(JsonConvert.SerializeObject(resultT70), "application/json");
                case "54YuCi":
                    mgName = "YuCi";
                    mgImg = Url.Content("~/images/YuCi.png");
                    var resultYuCi = new
                    {
                        mgName = mgName,
                        mgImg = mgImg,
                        mgJS = mgJS
                    };
                    return Content(JsonConvert.SerializeObject(resultYuCi), "application/json");
                case "54throat":
                    mgName = "throat";
                    mgImg = Url.Content("~/images/throat.png");
                    var resultthroat = new
                    {
                        mgName = mgName,
                        mgImg = mgImg,
                        mgJS = mgJS
                    };
                    return Content(JsonConvert.SerializeObject(resultthroat), "application/json");
                case "54sanae":
                    mgName = "sanae";
                    mgImg = Url.Content("~/images/sanae.png");
                    var resultsanae = new
                    {
                        mgName = mgName,
                        mgImg = mgImg,
                        mgJS = mgJS
                    };
                    return Content(JsonConvert.SerializeObject(resultsanae), "application/json");
                case "54Logout":
                    mgJS = @"var form = new FormData();
                    sessionStorage.removeItem('isManager');
                    logout.value = true;
                    fetch(`" + Url.Action("isMG", "Home") + @"`, {
                        method: 'POST',
                        body: form,
                    }).then(function(response) {
                    }).catch(function(err) {
                        alert(err);
                    });
                    setTimeout(function() {
                        window.location.href = '/';
                    }, 500);";
                    return Content(JsonConvert.SerializeObject(mgJS), "application/json");
                default:
                    return Content(JsonConvert.SerializeObject("密碼錯誤"), "application/json");
            }
        }

        public IActionResult WhereIsQueuely()
        {
            return View();
        }

    }
}
