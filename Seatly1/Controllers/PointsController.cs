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
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //點數商城導覽列
        public async Task<IActionResult> pointsShopContentHead()
        {
            return PartialView("_pointsShopContentHeadPartial", await _context.PointStores.OrderBy(s => s.Category).ToListAsync());
        }

        //點數商城內容
        public async Task<IActionResult> pointsShopContentBody(string? cate, int? pgNum, int? pgSize, string? keyword)
        {
            var isMg = HttpContext.Session.GetString("isMg"); //管理員登入判定

            int skipCount = ((int)pgNum - 1) * (int)pgSize;
            var PSCategories = await _context.PointStores.Select(s => s.Category).Distinct().ToListAsync();
            ViewBag.PSCategory = new SelectList(PSCategories);
            if (keyword != null)
            {
                var data = await _context.PointStores
                    .Where(s => s.ProductName.Contains(keyword))
                    .OrderBy(s => s.Category)
                    .ThenBy(s => s.ProductPrice)
                    .Skip(skipCount)
                    .Take((int)pgSize)
                    .ToListAsync();
                if (isMg == "true")
                {
                    data = await _context.PointStores
                    .Where(s => s.ProductName.Contains(keyword))
                    .OrderBy(s => s.ProductId)
                    .Skip(skipCount)
                    .Take((int)pgSize)
                    .ToListAsync();
                }

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
                    if (isMg == "true")
                    {
                        data = await _context.PointStores
                        .OrderBy(s => s.ProductId)
                        .Skip(skipCount)
                        .Take((int)pgSize)
                        .ToListAsync();
                    }

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

        //點數商城兌換
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

        //點數商城編輯
        [HttpPost]
        public async Task<IActionResult> EditPointsShop([FromBody] List<PointStore> pShop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (pShop.Count == 1 && pShop[0].ProductId == 0)
                    {
                        _context.Add(pShop[0]);
                    }
                    else
                    {
                        foreach (var pPd in pShop)
                        {
                            _context.Update(pPd);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle exception
                }
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePointsShop([FromBody]List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var pPd = await _context.PointStores.FindAsync(id);
                    if (pPd != null)
                    {
                        _context.PointStores.Remove(pPd);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        //點數商城上傳照片
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            // 处理图片上传逻辑，例如保存到服务器上的某个位置
            // 这里只是一个简单的示例，将图片保存到 wwwroot/uploads 文件夹下
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = image.FileName; // 保留原始檔名
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }

        // 點數商城刪除照片
        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromBody]List<string> fileNames)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            try
            {
                foreach (var fileName in fileNames)
                {
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        await Task.Run(() => System.IO.File.Delete(filePath));
                    }
                    else
                    {
                    }
                }
                return Ok($"Files deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting file: {ex.Message}");
            }
        }

        //點數商城兌換modal
        public async Task<IActionResult> pointsShopModal(int? id)
        {
            return PartialView("_pointsShopModalPartial", await _context.PointStores.FirstOrDefaultAsync(s => s.ProductId == id));
        }


        //點數交易紀錄導覽列
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

        //點數交易紀錄內容
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

        //現有優惠券導覽
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

        //現有優惠券內容
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

        //優惠券使用modal
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

        //優惠券使用post
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
