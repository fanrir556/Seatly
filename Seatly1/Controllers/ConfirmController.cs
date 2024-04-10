using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Seatly1.Models;
using Seatly1.ViewModels;

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
        public IActionResult Index(int? NId, string? UId)
        {
            var viewModel = new NotificationBookReader();


            BookingOrder? BookData = BookO(NId, UId);
            var notificationRecord = _context.NotificationRecords.FirstOrDefault(n => n.ActivityId == NId);

            viewModel.BookingOrder = BookData;
            viewModel.NotificationRecord = notificationRecord;

            return View(viewModel);
        }


        private BookingOrder BookO(int? NId ,string? UId)
        {
            var NontiData = _context.NotificationRecords.FirstOrDefault(n => n.ActivityId == NId);
            var UserData = _context.AspNetUsers.FirstOrDefault(u => u.UserName == UId);
            var newBookingOrder = new BookingOrder();

            if (NontiData != null)
            {
                newBookingOrder.ActivityId = NontiData.ActivityId;

                newBookingOrder.ActivityName = NontiData.ActivityName;
            }

            if (newBookingOrder.WaitingNumber != null)
            {
                newBookingOrder.WaitingNumber = newBookingOrder.WaitingNumber + 1;
            }
            else
            {
                newBookingOrder.WaitingNumber = 1;
            }

            if (UserData.UserName != null) 
            {
                newBookingOrder.UserName = UserData.UserName;
            }


            newBookingOrder.DateTime = DateTime.Now;
            newBookingOrder.Status = "UnCheck";


            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string barcode = new string(Enumerable.Range(0, 6)
           .Select(_ => chars[random.Next(chars.Length)]).ToArray());

            newBookingOrder.ActivityBarcode = barcode;
            newBookingOrder.Checked = false;



            return newBookingOrder;
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
