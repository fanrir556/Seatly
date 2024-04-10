using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    public class PointsController : Controller
    {
        SeatlyContext _context;

        public PointsController(SeatlyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET: /Points/pointsShopTitle
        public IActionResult pointsShopTitle()
        {
            return PartialView("_pointsShopTitlePartial");
        }

        public IActionResult pointsHistoryTitle()
        {
            return PartialView("_pointsHistoryTitlePartial");
        }

        public IActionResult couponTitle()
        {
            return PartialView("_couponTitlePartial");
        }

        public async Task<IActionResult> pointsShopContentHead()
        {
            return PartialView("_pointsShopContentHeadPartial", await _context.PointStores.OrderBy(s => s.Category).ToListAsync());
        }

        public async Task<IActionResult> pointsShopContentBody(string? cate, int? pgNum, int? pgSize)
        {
            if (cate == "all")
            {
                int skipCount = ((int)pgNum - 1) * (int)pgSize;

                var data = await _context.PointStores
                    .OrderBy(s => s.Category)
                    .ThenBy(s => s.ProductPrice)
                    .Skip(skipCount)
                    .Take((int)pgSize)
                    .ToListAsync();
                return PartialView("_pointsShopContentBodyPartial", data);
                //return PartialView("_pointsShopContentBodyPartial", await _context.PointStores.ToListAsync());
            }
            else
            {
                int skipCount = ((int)pgNum - 1) * (int)pgSize;

                var data = await _context.PointStores
                    .Where(s => s.Category == cate)
                    .OrderBy(s => s.Category)
                    .ThenBy(s => s.ProductPrice)
                    .Skip(skipCount)
                    .Take((int)pgSize)
                    .ToListAsync();
                return PartialView("_pointsShopContentBodyPartial", data);
                //return PartialView("_pointsShopContentBodyPartial", await _context.PointStores.Where(s => s.Category == cate).OrderBy(s => s.Category).ThenBy(s => s.ProductPrice).ToListAsync());
            }
        }

        public IActionResult pointsHistoryContent()
        {
            return PartialView("_pointsHistoryContentPartial");
        }

        public IActionResult couponContent()
        {
            return PartialView("_couponContentPartial");
        }
    }
}
