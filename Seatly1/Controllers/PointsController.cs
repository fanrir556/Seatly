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
using System.Diagnostics;

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

        public IActionResult VueIndex()
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
        //點數商城刪除
        [HttpPost]
        public async Task<IActionResult> DeletePointsShop([FromBody] List<int> ids)
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
        public async Task<IActionResult> DeleteImage([FromBody] List<string> fileNames)
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


        /*------------------------------------------現有優惠券------------------------------------------*/
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


        /*------------------------------------------點數交易紀錄------------------------------------------*/
        //點數交易紀錄導覽列
        public async Task<IActionResult> pointsHistoryContent()
        {
            var isMg = HttpContext.Session.GetString("isMg"); //管理員登入判定

            if (isMg == "true")
            {
                var data = new pointsShopViewModel
                {
                    pointsShopPd = await _context.PointStores.ToListAsync(),
                    trans = await _context.PointTransactions.OrderBy(s => s.Id).ToListAsync()
                };
                return PartialView("_pointsHistoryContentPartial", data);
            }
            else
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
        }

        //點數交易紀錄內容
        public async Task<IActionResult> pointsHistoryContentBody(int? pgNum, int? pgSize, string? keyword, string? cate)
        {
            var isMg = HttpContext.Session.GetString("isMg"); //管理員登入判定
            int skipCount = ((int)pgNum - 1) * (int)pgSize;
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                userId = user.Id;
            }

            if (isMg == "true")
            {
                //var active = await _context.PointTransactions.Select(s => s.Active).Distinct().ToListAsync();
                //ViewBag.active = new SelectList(active);
                var mId = await _context.AspNetUsers.Select(s => s.Id).Distinct().ToListAsync();
                var pId = await _context.PointStores.Select(s => s.ProductId).Distinct().ToListAsync();
                ViewBag.mId = new SelectList(mId);
                ViewBag.pId = new SelectList(pId);

                var data = new pointsShopViewModel();
                if (keyword != null)
                {
                    data.pointsShopPd = await _context.PointStores.ToListAsync();
                    if (cate == "mId")
                    {
                        data.trans = await _context.PointTransactions
                            .Where(s => s.MemberId.Contains(keyword))
                            .OrderBy(s => s.Id)
                            .Skip(skipCount)
                            .Take((int)pgSize)
                            .ToListAsync();
                    }
                    else
                    {
                        int Id = Int32.Parse(keyword);
                        data.trans = await _context.PointTransactions
                            .Where(s => s.Id == Id)
                            .OrderBy(s => s.Id)
                            .Skip(skipCount)
                            .Take((int)pgSize)
                            .ToListAsync();
                    }

                }
                else
                {
                    data.pointsShopPd = await _context.PointStores.ToListAsync();
                    data.trans = await _context.PointTransactions
                        .OrderBy(s => s.Id)
                        .Skip(skipCount)
                        .Take((int)pgSize)
                        .ToListAsync();
                }
                return PartialView("_pointsHistoryContentBodyPartial", data);
            }
            else
            {
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
        }


        //交易紀錄編輯
        [HttpPost]
        public async Task<IActionResult> EditPointsTran([FromBody] List<pShopExchange> pTrans)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (pTrans.Count == 1 && pTrans[0].Id == "0")
                    {
                        var newTran = new PointTransaction
                        {
                            MemberId = pTrans[0].MemberId,
                            ProductId = pTrans[0].ProductId,
                            Active = pTrans[0].Active,
                            TransactionDate = DateTime.Now
                        };
                        if (pTrans[0].tDate != null)
                        {
                            DateTime.TryParse(pTrans[0].tDate, out DateTime tDate);
                            newTran.TransactionDate = tDate;
                        }
                        _context.Add(newTran);
                    }
                    else
                    {
                        foreach (var pTran in pTrans)
                        {
                            int Id = Int32.Parse(pTran.Id);
                            var tran = await _context.PointTransactions.FindAsync(Id);
                            tran.Active = pTran.Active;
                            _context.Update(tran);
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
        //交易紀錄刪除
        [HttpPost]
        public async Task<IActionResult> DeletePointsTran([FromBody] List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var pTran = await _context.PointTransactions.FindAsync(id);
                    if (pTran != null)
                    {
                        _context.PointTransactions.Remove(pTran);
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

        public async Task<IActionResult> CheckIn()
        {
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
                    var dCheckIn = await _context.DailyCheckIns.FirstOrDefaultAsync(s => s.MemberId == user.Id && s.CheckInTime == date);
                    if (dCheckIn == null)
                    {
                        int ranNum = new Random().Next(1, 11);
                        int getPoints = 0;
                        if (ranNum == 10)
                        {
                            getPoints = 4;
                        }
                        else if (ranNum > 7 && ranNum <= 9)
                        {
                            getPoints = 3;
                        }
                        else if (ranNum > 4 && ranNum <= 7)
                        {
                            getPoints = 2;
                        }
                        else
                        {
                            getPoints = 1;
                        }

                        if (aspUser.Points == null)
                        {
                            aspUser.Points = 0;
                            aspUser.Points += getPoints;
                        }
                        else
                        {
                            aspUser.Points += getPoints;
                        }

                        var newCheckIn = new DailyCheckIn
                        {
                            Id = 0,
                            MemberId = aspUser.Id,
                            CheckInTime = date,
                        };

                        _context.Update(aspUser);
                        _context.Add(newCheckIn);
                        await _context.SaveChangesAsync();

                        List<int> res = new List<int> { getPoints, (int)aspUser.Points };
                        return Json(res);
                    }
                    return Json("今日已簽到");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> GamePoints()
        {
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
                    var gameCountList = await _context.GamePoints.Where(s => s.MemberId == user.Id && s.PointsDate == date).ToListAsync();
                    int gameCount = gameCountList.Count;
                    if (gameCount < 3)
                    {
                        int ranNum = new Random().Next(1, 11);
                        int getPoints = 0;
                        if (ranNum == 10)
                        {
                            getPoints = 4;
                        }
                        else if (ranNum > 7 && ranNum <= 9)
                        {
                            getPoints = 3;
                        }
                        else if (ranNum > 4 && ranNum <= 7)
                        {
                            getPoints = 2;
                        }
                        else
                        {
                            getPoints = 1;
                        }

                        if (aspUser.Points == null)
                        {
                            aspUser.Points = 0;
                            aspUser.Points += getPoints;
                        }
                        else
                        {
                            aspUser.Points += getPoints;
                        }

                        var newGamePoint = new GamePoint
                        {
                            Id = 0,
                            MemberId = aspUser.Id,
                            PointsDate = date,
                        };

                        _context.Update(aspUser);
                        _context.Add(newGamePoint);
                        await _context.SaveChangesAsync();

                        List<int> res = new List<int> { getPoints, (int)aspUser.Points, gameCount + 1 };
                        return Json(res);
                    }
                    return Json("今日已完成小遊戲");
                }
            }
            return NotFound();
        }

    }
}
