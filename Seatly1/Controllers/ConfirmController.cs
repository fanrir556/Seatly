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
using Seatly1.DTO;

namespace Seatly1.Controllers
{
    public class ConfirmController : Controller
    {
        private readonly SeatlyContext _context;

        public ConfirmController(SeatlyContext context)
        {
            _context = context;
        }



        //排隊最後列表
        // GET: ConfirmController
        public IActionResult Index(int? NId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var viewModel = new NotificationBookReader();
                BookingOrder? BookData = BookO(NId);
                if (BookData == null)
                {
                    return View("~/Views/Confirm/NumberFull.cshtml");
                }
                var notificationRecord = _context.NotificationRecords.FirstOrDefault(n => n.ActivityId == NId);

                viewModel.BookingOrder = BookData;
                viewModel.NotificationRecord = notificationRecord;

                return View(viewModel);
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }


        //篩選排隊邏輯
        private BookingOrder BookO(int? NId)
        {
            var NontiData = _context.NotificationRecords.FirstOrDefault(n => n.ActivityId == NId);
            var loggedInUserName = User.Identity.Name;

            var newBookingOrder = new BookingOrder();



            if (NontiData != null)
            {
                newBookingOrder.ActivityId = NontiData.ActivityId;

                newBookingOrder.ActivityName = NontiData.ActivityName;
            }

            var BookingOrderWaitingNumber = _context.BookingOrders
                .Where(b => b.ActivityId == NId)
                .OrderBy(c => c.WaitingNumber)
                .LastOrDefault();

            if (BookingOrderWaitingNumber != null && BookingOrderWaitingNumber.WaitingNumber != null)
            {
                newBookingOrder.WaitingNumber = BookingOrderWaitingNumber.WaitingNumber + 1;
                if (newBookingOrder.WaitingNumber > NontiData.Capacity)
                {
                    return null;
                }

            }
            else
            {
                newBookingOrder.WaitingNumber = 1;
            }

            if (loggedInUserName != null)
            {
                newBookingOrder.UserName = loggedInUserName;
            }


            newBookingOrder.DateTime = DateTime.Now;
            newBookingOrder.Status = "UnCheck";


            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string barcode = new string(Enumerable.Range(0, 6)
           .Select(_ => chars[random.Next(chars.Length)]).ToArray());

            newBookingOrder.ActivityBarcode = barcode;
            newBookingOrder.Checked = false;

            AddBookingOrder(newBookingOrder);

            return newBookingOrder;
        }

        //寫入排隊
        private void AddBookingOrder(BookingOrder newBookingOrder)
        {
            try
            {
                _context.BookingOrders.Add(newBookingOrder);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // 處理寫入失敗的異常
                Console.WriteLine("寫入資料庫失敗：" + ex.Message);
                throw; // 可以選擇處理或再次拋出異常
            }
        }


        //活動檢查頁面-OrganizerActiveCheckIndex
        public IActionResult OrganizerActiveCheckIndex() {

            return View();
        }



        //OrganizerActiveCheckIndex使用
        //輸出活動列表
        //GET:/Confirm/ActiveList
        [HttpGet]
        public async Task<IEnumerable<NotificationRecordDTO>> ActiveList(int id) {
            var aa = await _context.NotificationRecords
                .Where(e => e.OrganizerId == id && e.IsActivity == true && e.EndTime > DateTime.Now)
                .Select(
                e => new NotificationRecordDTO
                {
                    ActivityId = e.ActivityId,
                    ActivityName = e.ActivityName,
                    ActivityPhoto = e.ActivityPhoto,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                }).ToListAsync();

            return aa;
        }

        //OrganizerActiveCheckIndex使用
        //改變活動欄位
        //POST:/Confirm/TransActive
        [HttpPost]
        public async Task TransActive(int id) 
        {
            var aa = _context.NotificationRecords.FirstOrDefault(e => e.ActivityId == id);
            aa.IsActivity = false;
            await _context.SaveChangesAsync();
        }


        //回傳排隊頁面-OrganizerActiveCheck
        //Get:/Confirm/OrganizerActiveCheck
        [HttpGet]
        public IActionResult OrganizerActiveCheck(int id) 
        {
            return View(id);
        }


        //OrganizerActiveCheck使用
        //輸出個別活動//輸出還是array
        //GET:/Confirm/ActiveInfo
        [HttpGet]
        public async Task<IEnumerable<NotificationRecordDTO>> ActiveInfo(int id)
        {
            var aa = await _context.NotificationRecords
                .Where(e => e.ActivityId == id)
                .Select(
                e => new NotificationRecordDTO
                {
                    ActivityId = e.ActivityId,
                    ActivityName = e.ActivityName,
                    ActivityPhoto = e.ActivityPhoto,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                }).ToListAsync();

            return aa;
        }


        //OrganizerActiveCheck使用
        //GET:/Confirm/BookInfo
        [HttpGet]
        public async Task<IEnumerable<BookingOrder>> BookInfo(int id)
        {
            var BookInfo = await _context.BookingOrders.Where(
                e => e.ActivityId == id).ToListAsync();
            
           return BookInfo;
             
        }

        //OrganizerActiveCheck使用
        //POST:/Confirm/TransCheck
        [HttpPost]
        public async Task TransCheck(string Barcode,int waitNum)
        {
            var aa = _context.BookingOrders.FirstOrDefault(
                    e => e.ActivityBarcode == Barcode && e.WaitingNumber == waitNum
                );
            aa.Checked = true;
            await _context.SaveChangesAsync();
        }

        //管理者使用 -
        //POST:/Confirm/TransUnCheck
        [HttpPost]
        public async Task TransUnCheck(string Barcode, int waitNum)
        {
            var aa = _context.BookingOrders.FirstOrDefault(
                    e => e.ActivityBarcode == Barcode && e.WaitingNumber == waitNum
                );
            aa.Checked = false;
            await _context.SaveChangesAsync();
        }


        //呼叫活動歷史頁面OrganizerActiveHistoryIndex
        //GET:Confirm/OrganizerActiveHistoryIndex
        [HttpGet]
        public IActionResult OrganizerActiveHistoryIndex()
        {
            return View();
        }



        //OrganizerActiveHistoryIndex使用
        //輸出活動列表
        //GET:/Confirm/ActiveList
        [HttpGet]
        public async Task<IEnumerable<NotificationRecordDTO>> ActiveHistoryList(int id)
        {
            var aa = await _context.NotificationRecords
                .Where(e => e.OrganizerId == id && e.EndTime < DateTime.Now && e.IsActivity == false)

                .Select(
                e => new NotificationRecordDTO
                {
                    ActivityId = e.ActivityId,
                    ActivityName = e.ActivityName,
                    ActivityPhoto = e.ActivityPhoto,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                }).ToListAsync();

            return aa;
        }

        //OrganizerActiveHistoryIndex使用
        //輸出活動列表
        //GET:/Confirm/OrganizerActiveHistory_CheckInfo
        [HttpGet]
        public IActionResult OrganizerActiveHistory_CheckInfo() 
        {
            return View();
        }

        //呼叫OrganizerActiveHistory_ActInfo
        //活動資訊
        //GET:/Confirm/OrganizerActiveHistory_ActInfo
        [HttpGet]
        public IActionResult OrganizerActiveHistory_ActInfo()
        {
            return View();
        }



    }
}
