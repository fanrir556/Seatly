using Microsoft.AspNetCore.Mvc;
using Seatly1.DTO;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    public class RestaurantController : Controller
    {
        SeatlyContext _context;
        public RestaurantController(SeatlyContext context)
            {
            _context = context;
            }

        [HttpPost]
        public async Task<string> PostRestaurant(RestaurantDTO Restaurant)
        {
            Restaurant emp = new Restaurant
            {
                RestaurantId = Restaurant.RestaurantId,
                RestaurantAccount = Restaurant.RestaurantAccount,
            };
            _context.Restaurant.Add(emp);
            await _context.SaveChangesAsync();
            return $"新增成功餐廳編號:{emp.RestaurantId}";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
