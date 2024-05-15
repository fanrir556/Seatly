using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class CollectionsController : Controller
    {
        SeatlyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CollectionsController(SeatlyContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            // 获取当前登录用户的 UserID
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            // 将 UserID 存储在 Session 中
            _httpContextAccessor.HttpContext.Session.SetString("UserId", userId);

            // 输出 Session 中的值
            var userIdInSession = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            Debug.WriteLine("儲存的UserID: " + userIdInSession);

            return RedirectToPage("/Areas/Identity/Pages/Account/Manage/Collections.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(CollectionItem collectionItem)
        {
            if (ModelState.IsValid)
            {
                // 添加收藏信息到数据库
                _context.CollectionItems.Add(collectionItem);
                await _context.SaveChangesAsync();

                return Ok(); // 或者可以返回其他状态或数据
            }

            return BadRequest(); // 如果模型验证失败则返回错误状态
        }
    }
}
