using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    public class RestaurantTableController : Controller
    {
        private readonly ILogger<RestaurantTableController> _logger;

        public RestaurantTableController(ILogger<RestaurantTableController> logger)
        {
            _logger = logger;
        }

        // GET: RestaurantTable
        public IActionResult Index()
        {
            return View();
        }

        // GET: RestaurantTable/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: RestaurantTable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantTable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RestaurantTable/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: RestaurantTable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RestaurantTable/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: RestaurantTable/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
