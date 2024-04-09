using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    public class ConfirmController : Controller
    {
        private readonly SeatlyContext _context;

        public ConfirmController(SeatlyContext context)
        {
            _context = context;
        }


      

        // GET: ConfirmController
        public ActionResult Index()
        {
            BookingOrder? BookData = BookO();

            return View(BookData);
        }


        private BookingOrder BookO()
        {
            var NontiData = _context.NotificationRecords.FirstOrDefault();
            var UserData = _context.AspNetUsers.FirstOrDefault();
            var BookData = _context.BookingOrders.FirstOrDefault();

            if (NontiData != null)
            { 
            BookData.ActivityId = NontiData.ActivityId;
            BookData.ActivityName = NontiData.ActivityName;
            }

            if (BookData.WaitingNumber != null)
            {
                BookData.WaitingNumber = BookData.WaitingNumber + 1;
            }
            else
            {
                BookData.WaitingNumber = 1;
            }

            if (UserData.UserName != null) 
            { 
            BookData.UserName = UserData.UserName;
            }


            BookData.DateTime = DateTime.Now;
            BookData.Status = "UnCheck";


            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string barcode = new string(Enumerable.Range(0, 6)
           .Select(_ => chars[random.Next(chars.Length)]).ToArray());

            BookData.ActivityBarcode = barcode;
            BookData.Checked = false;
            return BookData;
        }

        // GET: ConfirmController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ConfirmController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConfirmController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ConfirmController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConfirmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ConfirmController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConfirmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
