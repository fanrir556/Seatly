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

        //GET: /Points/Title
        public IActionResult pointsShopTitle()
        {
            return PartialView("_pointsShopTitlePartial");
        }
    }
}
