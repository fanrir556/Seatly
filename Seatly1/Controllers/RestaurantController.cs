using Microsoft.AspNetCore.Mvc;
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

        //GET: Restaurant/Greet 
        [HttpGet]
        public string Greet(string Name) 
        {
            return $"Hello,{Name}!"; 
        }

        //POST: Restaurant/Greet 
        [HttpPost,ActionName("Greet")]
        public string PostGreet(string Name) 
        {
            return $"Hello,{Name}!";
        }

        [HttpPost()]
        public string FetchPostGreet([FromBody]Parameter p)
        {
            return $"Hello,{p.Name}!";
        }

        ////POST: /Ajax/CheckRestaurantName
        //[HttpPost()]
        //public string CheckRestaurantName(string RestaurantName)
        //{
        //    bool Exists = _context.Restaurants.Any(emp => emp.RestaurantName == RestaurantName);
        //    return Exists ? "true" : "false" ;   
        //}

        //[HttpPost()]
        //public string FetchCheckRestaurantName(string RestaurantName)
        //{
        //    bool Exists = _context.Restaurants.Any(emp => emp.RestaurantName == RestaurantName);
        //    return Exists ? "true" : "false";
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
