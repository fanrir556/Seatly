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
    [EnableCors("AllowAny")]
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

            /*var products = !string.IsNullOrEmpty(search.Keyword) ? _context.PointStores.Where(s => s.ProductName.Contains(search.Keyword)) : _context.PointStores;

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
            }*/


            //products = products.Skip(skipCount).Take(pageSize);

            var products = _context.PointStores;

            //分頁
            int totalCount = products.Count();
            int pageNum = search.PgNum ?? 1;
            int pageSize = search.PgSize ?? 10;
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            int skipCount = (pageNum - 1) * pageSize;

            PointsPagingDTO pointsPaging = new PointsPagingDTO();
            pointsPaging.TotalPages = totalPages;
            pointsPaging.Shops = await products.ToListAsync();

            pointsPaging.SList1 = await _context.PointStores.Select(s => s.Category).Distinct().ToListAsync();

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

            List<string> DNames = new List<string>();
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

            if (isMg == "true")
            {
                pointsPaging.isMg = true;
            }
            
            return pointsPaging;
        }

        //點數商城VUE
        [HttpPost("coupon")]
        public async Task<ActionResult<PointsPagingDTO>> couponVue([FromBody] PointsSearchDTO search)
        {
            var isMg = HttpContext.Session.GetString("isMg"); //管理員登入判定

            //var trans = !string.IsNullOrEmpty(search.Keyword) ? (search.SearchBy == "id" ? _context.PointTransactions.Where(s => s.Id == int.Parse(search.Keyword)) : _context.PointTransactions.Where(s => s.MemberId.Contains(search.Keyword))) : _context.PointTransactions;

            //switch (search.SortBy)
            //{
            //    case "id":
            //        trans = search.SortType == "asc" ? trans.OrderBy(s => s.Id) : trans.OrderByDescending(s => s.Id);
            //        break;
            //    case "mid":
            //        trans = search.SortType == "asc" ? trans.OrderBy(s => s.MemberId) : trans.OrderByDescending(s => s.MemberId);
            //        break;
            //    case "date":
            //        trans = search.SortType == "asc" ? trans.OrderBy(s => s.TransactionDate) : trans.OrderByDescending(s => s.TransactionDate);
            //        break;
            //    default:
            //        trans = search.SortType == "asc" ? trans.OrderBy(s => s.Id) : trans.OrderByDescending(s => s.Id);
            //        break;
            //}

            //if (User.Identity.IsAuthenticated && isMg != "true")
            //{
            //    // 使用者已登入
            //    // 在這裡進行相關處理
            //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //    var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
            //    if (aspUser != null)
            //    {
            //        trans = trans.Where(s => s.MemberId == aspUser.Id);
            //    }
            //}

            var trans = await _context.PointTransactions.ToListAsync();
            //分頁
            int totalCount = trans.Count();
            int pageNum = search.PgNum ?? 1;
            int pageSize = search.PgSize ?? 10;
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            int skipCount = (pageNum - 1) * pageSize;

            //trans = trans.Skip(skipCount).Take(pageSize);
            var products = await _context.PointStores.ToListAsync();

            PointsPagingDTO pointsPaging = new PointsPagingDTO();
            pointsPaging.TotalPages = totalPages;
            pointsPaging.Trans = trans;
            pointsPaging.Shops = products;

            pointsPaging.SList1 = await _context.AspNetUsers.Select(s => s.Id).ToListAsync();
            pointsPaging.IList1 = await _context.PointStores.Select(s => s.ProductId).ToListAsync();

            if (User.Identity.IsAuthenticated && isMg != "true")
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                if (aspUser != null)
                {
                    pointsPaging.UserId = aspUser.Id;
                }
            }

            List<string> DNames = new List<string>();
            // 取得 PointStore 類別
            Type pointTrans = typeof(PointTransaction);

            // 取得 PointStore 類別的所有屬性
            PropertyInfo[] propertyInfos = pointTrans.GetProperties();

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

            if (isMg == "true")
            {
                pointsPaging.isMg = true;
            }

            return pointsPaging;
        }

        //點數商城VUE
        [HttpGet("userPoints")]
        public async Task<int> userPointsVue()
        {
            if (User.Identity.IsAuthenticated)
            {
                // 使用者已登入
                // 在這裡進行相關處理
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var aspUser = await _context.AspNetUsers.FindAsync(user.Id);
                if (aspUser.Points == null)
                {
                    return 0;
                }
                else
                {
                    return (int)aspUser.Points;
                }
            }
            return -1;
        }

    }
}
