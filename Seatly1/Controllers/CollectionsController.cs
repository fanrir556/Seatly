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
        }

        public CollectionsController(SeatlyContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> CollectionsIndex()
        {

            // 获取当前登录用户的 UserID
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id ?? throw new UnauthorizedAccessException(); // 显式处理 null

            if (userId == null)
            {
                return Unauthorized(); // 用户未登录
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
                                         ActivityName = n.ActivityName
                                     }).ToListAsync();

            // 输出 Session 中的值
            var userIdInSession = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            Debug.WriteLine("儲存的UserID: " + userIdInSession);

            return View(collections);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddToCollection(CollectionItem collectionItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.CollectionItems.Add(collectionItem);
        //        await _context.SaveChangesAsync();

        //        return Json(new { success = true }); // Return a valid JSON response
        //    }

        //    return Json(new { success = false, error = "Model validation failed" }); // Return error details as JSON
        //}

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int activityId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // 用户未登录
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
                return Json(new { success = true }); // 返回有效的 JSON 响应
            }

            return Json(new { success = false, error = "模型验证失败" }); // 返回错误详情的 JSON 响应
        }
    }
}
