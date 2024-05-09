using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.DTO;
using Seatly1.Models;
using Seatly1.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Seatly1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsAPIController : ControllerBase
    {
        SeatlyContext _context;
        UserManager<ApplicationUser> _userManager;

        public PointsAPIController(SeatlyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //點數商城VUE
        public async Task<ActionResult<PointsPagingDTO>> pointsShopVue([FromBody] PointsSearchDTO search)
        {
            var isMg = HttpContext.Session.GetString("isMg"); //管理員登入判定

            var products = !string.IsNullOrEmpty(search.Keyword) ? _context.PointStores.Where(s => s.ProductName.Contains(search.Keyword)) : _context.PointStores;

            products = search.Cate != null ? products.Where(s => s.Category == search.Cate) : products;

            switch (search.SortBy)
            {
                case "id":
                    products = search.SortType == "asc" ? products.OrderBy(s => s.ProductId) : products.OrderByDescending(s => s.ProductId);
                    break;
                case "price":
                    products = search.SortType == "asc" ? products.OrderBy(s => s.ProductPrice) : products.OrderByDescending(s => s.ProductPrice);
                    break;
                case "cate":
                    products = search.SortType == "asc" ? products.OrderBy(s => s.Category).ThenBy(s => s.ProductPrice) : products.OrderByDescending(s => s.Category).ThenBy(s => s.ProductPrice);
                    break;
                default:
                    products = search.SortType == "asc" ? products.OrderBy(s => s.Category).ThenBy(s => s.ProductPrice) : products.OrderByDescending(s => s.Category).ThenBy(s => s.ProductPrice);
                    break;
            }

            //分頁
            int totalCount = products.Count();
            int pageNum = search.PgNum ?? 1;
            int pageSize = search.PgSize ?? 10;
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            int skipCount = (pageNum - 1) * pageSize;

            products = products.Skip(skipCount).Take(pageSize);

            PointsPagingDTO pointsPaging = new PointsPagingDTO();
            var PSCategories = await _context.PointStores.Select(s => s.Category).Distinct().ToListAsync();
            pointsPaging.List1 = new SelectList(PSCategories);
            pointsPaging.TotalPages = totalPages;
            pointsPaging.Shops = await products.ToListAsync();

            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                pointsPaging.UserPoints = aspUser.Points;
            }

            return pointsPaging;
        }
    }
}
