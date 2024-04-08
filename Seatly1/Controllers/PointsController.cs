using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Seatly1.Controllers
{
    public class PointsController : Controller
    {
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
    }
}
