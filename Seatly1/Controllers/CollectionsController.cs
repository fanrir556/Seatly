using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace Seatly1.Controllers
{
    public class CollectionsController : Controller
    {
        SeatlyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public class CollectionViewModel
        {
            public int ActivityId { get; set; }
            public string ActivityName { get; set; }
            public byte[] ActivityPhoto {  get; set; }
            public string Location { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string HashTag1 { get; set; }
            public string HashTag2 { get; set; }
            public string HashTag3 { get; set; }
            public string HashTag4 { get; set; }
            public string HashTag5 { get; set; }
            public string DescriptionN { get; set; }
        }

        public class RemoveCollectionRequest
        {
            public int ActivityId { get; set; }
        }

        public CollectionsController(SeatlyContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> CollectionsIndex()
        {

            // 取得用户的 UserID
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id ?? throw new UnauthorizedAccessException(); // 處理 null

            if (userId == null)
            {
                return Unauthorized(); // 用戶未登錄
            }

            // 将 UserID 存储在 Session 中
            _httpContextAccessor.HttpContext.Session.SetString("UserId", userId);

            // 获取用户的收藏项，并联接到 NotificationRecords 获取活动名称
            var collections = await (from c in _context.CollectionItems
                                     join n in _context.NotificationRecords
                                     on c.ActivityId equals n.ActivityId
                                     where c.UserId == userId
                                     select new CollectionViewModel
                                     {
                                         ActivityId = (int)c.ActivityId,
                                         ActivityName = n.ActivityName,
                                         ActivityPhoto = n.ActivityPhoto,
                                         Location = n.Location,
                                         StartTime = (DateTime)n.StartTime,
                                         EndTime = (DateTime)n.EndTime,
                                         HashTag1 = n.HashTag1,
                                         HashTag2 = n.HashTag2,
                                         HashTag3 = n.HashTag3,
                                         HashTag4 = n.HashTag4,
                                         HashTag5 = n.HashTag5,
                                         DescriptionN = n.DescriptionN,
                                     }).ToListAsync();

            // 输出 Session 中的值
            var userIdInSession = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            Debug.WriteLine("儲存的UserID: " + userIdInSession);

            return View(collections);
        }

        

        // 加入收藏
        [HttpPost]
        public async Task<IActionResult> AddToCollection(int activityId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // 用户未登录
            }

            var userId = user.Id;

            // 检查是否已经存在该收藏项
            var existingItem = await _context.CollectionItems
                .FirstOrDefaultAsync(c => c.ActivityId == activityId && c.UserId == userId);

            if (existingItem != null)
            {
                return Json(new { success = false, message = "已經收藏過囉~" });
            }

            var collectionItem = new CollectionItem
            {
                ActivityId = activityId,
                UserId = user.Id
            };

            if (ModelState.IsValid)
            {
                _context.CollectionItems.Add(collectionItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "收藏成功!" }); // 返回有效的 JSON 响应
            }

            return Json(new { success = false, error = "模型驗證失敗" }); // 返回错误详情的 JSON 响应
        }

        // 移除收藏
        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection([FromBody] RemoveCollectionRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // 用户未登录
            }

            var userId = user.Id;
            Debug.WriteLine($"Attempting to remove collection item with ActivityId: {request.ActivityId} for UserId: {userId}");

            var collectionItem = await _context.CollectionItems
                .FirstOrDefaultAsync(c => c.ActivityId == request.ActivityId && c.UserId == user.Id);

            if (collectionItem != null)
            {
                _context.CollectionItems.Remove(collectionItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true }); // 返回有效的 JSON 回應
            }

            return Json(new { success = false, error = "未找到收藏项" }); 
        }
    }
}
