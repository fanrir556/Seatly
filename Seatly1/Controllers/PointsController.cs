using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; //使用者登入管理
using Seatly1.Models;
using Seatly1.Data;
using System.Security.Claims;
using Seatly1.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Imaging;
using System.Drawing;
using QRCoder;

namespace Seatly1.Controllers
{
    public class PointsController : Controller
    {
        SeatlyContext _context;
        //SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;

        public PointsController(SeatlyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> pointsShopContentHead()
        {
            return PartialView("_pointsShopContentHeadPartial", await _context.PointStores.OrderBy(s => s.Category).ToListAsync());
        }

        public async Task<IActionResult> pointsShopContentBody(string? cate, int? pgNum, int? pgSize, string? keyword)
        {
            //var isMg = HttpContext.Session.GetString("isMg"); 管理員登入判定

            int skipCount = ((int)pgNum - 1) * (int)pgSize;
            if (keyword != null)
            {
                var data = await _context.PointStores
                    .Where(s => s.ProductName.Contains(keyword))
                    .OrderBy(s => s.Category)
                    .ThenBy(s => s.ProductPrice)
                    .Skip(skipCount)
                    .Take((int)pgSize)
                    .ToListAsync();

                if (User.Identity.IsAuthenticated)
                {
                    // 使用者已登入
                    // 在這裡進行相關處理
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                    var pShop = new pointsShopViewModel
                    {
                        pointsShopPd = data,
                        user = aspUser
                    };

                    return PartialView("_pointsShopContentBodyPartial", pShop);
                }
                else
                {
                    // 使用者未登入
                    // 在這裡進行相關處理
                    var pShop = new pointsShopViewModel
                    {
                        pointsShopPd = data,
                    };
                    return PartialView("_pointsShopContentBodyPartial", pShop);
                }
            }
            else
            {
                if (cate == "all")
                {
                    var data = await _context.PointStores
                        .OrderBy(s => s.Category)
                        .ThenBy(s => s.ProductPrice)
                        .Skip(skipCount)
                        .Take((int)pgSize)
                        .ToListAsync();

                    if (User.Identity.IsAuthenticated)
                    {
                        // 使用者已登入
                        // 在這裡進行相關處理
                        var user = await _userManager.FindByNameAsync(User.Identity.Name);
                        var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                        var pShop = new pointsShopViewModel
                        {
                            pointsShopPd = data,
                            user = aspUser
                        };

                        return PartialView("_pointsShopContentBodyPartial", pShop);
                    }
                    else
                    {
                        // 使用者未登入
                        // 在這裡進行相關處理
                        var pShop = new pointsShopViewModel
                        {
                            pointsShopPd = data,
                        };
                        return PartialView("_pointsShopContentBodyPartial", pShop);
                    }
                }
                else
                {
                    var data = await _context.PointStores
                        .Where(s => s.Category == cate)
                        .OrderBy(s => s.Category)
                        .ThenBy(s => s.ProductPrice)
                        .Skip(skipCount)
                        .Take((int)pgSize)
                        .ToListAsync();

                    if (User.Identity.IsAuthenticated)
                    {
                        // 使用者已登入
                        // 在這裡進行相關處理
                        var user = await _userManager.FindByNameAsync(User.Identity.Name);
                        var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                        var pShop = new pointsShopViewModel
                        {
                            pointsShopPd = data,
                            user = aspUser
                        };

                        return PartialView("_pointsShopContentBodyPartial", pShop);
                    }
                    else
                    {
                        // 使用者未登入
                        // 在這裡進行相關處理
                        var pShop = new pointsShopViewModel
                        {
                            pointsShopPd = data,
                        };
                        return PartialView("_pointsShopContentBodyPartial", pShop);
                    }
                }
            }
        }

        [HttpPost]
        public async Task<string> pointsShopContentBody([FromBody] pShopExchange p)
        {
            //, AspNetUser aspUser, PointTransaction trans
            AspNetUser aspUser = await _context.AspNetUsers.FindAsync(p.Id);
            if (aspUser == null)
            {
                return "兌換失敗";
            }

            aspUser.Points = p.Points;
            PointTransaction trans = new PointTransaction
            {
                Id = 0,
                MemberId = p.MemberId,
                ProductId = p.ProductId,
                TransactionDate = DateTime.Now,
                Active = p.Active
            };
            if (ModelState.IsValid)
            {
                _context.Update(aspUser);
                _context.Add(trans);
                await _context.SaveChangesAsync();
                return "兌換成功";
            }
            else
            {
                return "兌換失敗";
            }
        }

        public async Task<IActionResult> pointsShopModal(int? id)
        {
            return PartialView("_pointsShopModalPartial", await _context.PointStores.FirstOrDefaultAsync(s => s.ProductId == id));
        }

        public async Task<IActionResult> pointsHistoryContent()
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                userId = user.Id;
            }
            var data = new pointsShopViewModel
            {
                pointsShopPd = await _context.PointStores.ToListAsync(),
                trans = await _context.PointTransactions.Where(s => s.MemberId == userId).ToListAsync()
            };
            return PartialView("_pointsHistoryContentPartial", data);
        }

        public async Task<IActionResult> pointsHistoryContentBody(int? pgNum, int? pgSize)
        {
            int skipCount = ((int)pgNum - 1) * (int)pgSize;
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                userId = user.Id;
            }
            var data = new pointsShopViewModel
            {
                pointsShopPd = await _context.PointStores.ToListAsync(),
                trans = await _context.PointTransactions
                        .Where(s => s.MemberId == userId)
                        .OrderBy(s => s.TransactionDate)
                        .Skip(skipCount)
                        .Take((int)pgSize)
                        .ToListAsync()
            };
            return PartialView("_pointsHistoryContentBodyPartial", data);
        }

        public async Task<IActionResult> couponContent()
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                userId = user.Id;
            }
            var data = new pointsShopViewModel
            {
                pointsShopPd = await _context.PointStores.ToListAsync(),
                trans = await _context.PointTransactions.Where(s => s.MemberId == userId && s.Active == true).ToListAsync()
            };
            return PartialView("_couponContentPartial", data);
        }

        public async Task<IActionResult> couponContentBody(int? pgNum, int? pgSize)
        {
            int skipCount = ((int)pgNum - 1) * (int)pgSize;
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                userId = user.Id;
            }
            var data = new pointsShopViewModel
            {
                pointsShopPd = await _context.PointStores.ToListAsync(),
                trans = await _context.PointTransactions
                        .Where(s => s.MemberId == userId && s.Active == true)
                        .OrderBy(s => s.ProductId)
                        .ThenBy(s => s.TransactionDate)
                        .Skip(skipCount)
                        .Take((int)pgSize)
                        .ToListAsync()
            };
            return PartialView("_couponContentBodyPartial", data);
        }

        public async Task<IActionResult> couponModal(int? id)
        {
            var pdId = await _context.PointTransactions.Where(s => s.Id == id).Select(s => s.ProductId).FirstOrDefaultAsync();
            var data = new pointsShopViewModel
            {
                pointsShopPd = await _context.PointStores.Where(s => s.ProductId == pdId).ToListAsync(),
                trans = await _context.PointTransactions.Where(s => s.Id == id).ToListAsync()
            };
            return PartialView("_couponModalModalPartial", data);
        }

        [HttpPost]
        public async Task<IActionResult> couponUse([FromBody] pShopExchange p)
        {
            int id = Int32.Parse(p.Id);
            PointTransaction trans = await _context.PointTransactions.FindAsync(id);
            if (trans == null)
            {
                return NotFound();
            }

            trans.Active = p.Active;

            if (ModelState.IsValid)
            {
                _context.Update(trans);
                await _context.SaveChangesAsync();

                string dataString = $"優惠券編號:{trans.Id},會員編號:{trans.MemberId},商品編號:{trans.ProductId},兌換日期:{trans.TransactionDate}";

                // 創建 QR code
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(dataString, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qrCode.GetGraphic(10);// 20為二維碼的大小

                // 將 QR code 作為圖片回傳
                using (MemoryStream ms = new MemoryStream(qrCodeBytes))
                {
                    return File(ms.ToArray(), "image/png");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
