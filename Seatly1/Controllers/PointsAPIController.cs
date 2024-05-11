using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.DTO;
using Seatly1.Models;
using Seatly1.ViewModels;
using System.ComponentModel;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Seatly1.Controllers
{
    [EnableCors("PointsAPI")]
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
        [HttpPost("pointsShop")]
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
            pointsPaging.TotalPages = totalPages;
            pointsPaging.Shops = await products.ToListAsync();

            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                if (aspUser.Points == null)
                {
                    pointsPaging.UserPoints = 0;
                }
                else
                {
                    pointsPaging.UserPoints = aspUser.Points;
                }
            }

            if (isMg == "true")
            {
                List<string> DNames = new List<string>();
                var PSCategories = await _context.PointStores.Select(s => s.Category).Distinct().ToListAsync();
                pointsPaging.SList1 = new SelectList(PSCategories);
                // 取得 PointStore 類別
                Type pointStoreType = typeof(PointStore);

                // 取得 PointStore 類別的所有屬性
                PropertyInfo[] propertyInfos = pointStoreType.GetProperties();

                // 遍歷所有屬性
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    // 取得屬性的 Display 屬性
                    DisplayNameAttribute displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();

                    // 如果屬性有 Display 屬性
                    if (displayNameAttribute != null)
                    {
                        // 取得屬性名稱和顯示名稱
                        string propertyName = propertyInfo.Name;
                        string displayName = displayNameAttribute.DisplayName;

                        DNames.Add(displayName);
                    }
                }
                pointsPaging.DNames = DNames;
                pointsPaging.isMg = true;
            }
            
            return pointsPaging;
        }
    }
}
