using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    public class CollectionsController : Controller
    {
        SeatlyContext _context;

        public CollectionsController(SeatlyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToPage("/Areas/Identity/Pages/Account/Manage/Collections.cshtml");
        }
    }
}
