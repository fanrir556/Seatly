using Microsoft.AspNetCore.Mvc;
using Seatly1.Data;
using Seatly1.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http; // 添加這行

namespace Seatly1.Controllers
{
    public class RouletteController : Controller
    {
        private readonly ILogger<RouletteController> _loggerRoulette;
        private readonly SeatlyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // 添加這行

        public RouletteController(ILogger<RouletteController> logger, SeatlyContext context, IHttpContextAccessor httpContextAccessor)
        {
            _loggerRoulette = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult RouletteIndex()
        {
            return View();
        }

        


        [HttpPost]
        public async Task<IActionResult> SpinWheel(string selectedTag)
        {
            
            // 檢查收到的 selectedTag 值
            Debug.WriteLine("收到的 selectedTag: " + selectedTag);
            try
            {
               
                // 檢查 selectedTag 是否為 null 或空
                if (string.IsNullOrEmpty(selectedTag))
                {
                    return Json(new { success = false, message = "未提供選擇的標籤" });
                }

                // 找符合的數據
                var randomRecord = await Task.Run(() =>
                {
                    return _context.NotificationRecords
                        .Where(record =>
                            record.HashTag1.Contains(selectedTag) ||
                            record.HashTag2.Contains(selectedTag) ||
                            record.HashTag3.Contains(selectedTag) ||
                            record.HashTag4.Contains(selectedTag) ||
                            record.HashTag5.Contains(selectedTag))
                        .OrderBy(x => Guid.NewGuid()) // 隨機排序
                        .FirstOrDefault();
                });

                if (randomRecord != null)
                {
                    byte[] imageBytes = randomRecord.ActivityPhoto; // 二進位圖片數據
                    string base64String = Convert.ToBase64String(imageBytes);

                    // 如果找到符合條件的記錄，返回相應的 Name 值
                    return Json(new { success = true, name = randomRecord.ActivityName, description = randomRecord.DescriptionN, photo = base64String,activityId = randomRecord.ActivityId });
                }

                return Json(new { success = false, message = "後端沒找到資料" });
            }
            catch (Exception ex)
            {
                _loggerRoulette.LogError(ex, "處理 SpinWheel 請求時發生錯誤");
                return Json(new { success = false, message = "處理請求時發生錯誤" });
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
