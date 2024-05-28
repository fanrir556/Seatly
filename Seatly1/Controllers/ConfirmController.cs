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
using Microsoft.AspNetCore.Identity;
using Seatly1.Data;
using static System.Net.WebRequestMethods;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Seatly1.Controllers
{
    public class ConfirmController : Controller
    {
        private readonly SeatlyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ConfirmController(SeatlyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // 定義一個新的枚舉類型來表示不同的錯誤類型
        public enum BookingResult
        {
            Success,
            UserAlreadyBooked,
            ActivityFull
        }

        // 排隊最後列表
        // GET:/Confirm/Index
        public IActionResult Index(int? NId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var viewModel = new NotificationBookReader();
                var (BookData, result) = BookO(NId);

                switch (result)
                {
                    case BookingResult.UserAlreadyBooked:
                        return Json(new { status = "UserAlreadyBooked", message = "您已經有參加活動了喔" ,url = Url.Action("BookOK", "Confirm") });

                    case BookingResult.ActivityFull:
                        return Json(new { status = "ActivityFull", message = "活動已滿，無法加入" , url = Url.Action("NumberFull", "Confirm") });

                    case BookingResult.Success:
                        return Json(new { status = "Success", url = Url.Action("BookOK", "Confirm") });

                    default:
                        return Json(new { status = "Error", message = "發生未知錯誤，請稍後再試" });
                }
            }
            else
            {
                return Json(new { status = "NotAuthenticated", message = "請先登入", url = "https://localhost:7271/Identity/Account/Login" });
            }
        }


        // 篩選排隊邏輯
        private (BookingOrder?, BookingResult) BookO(int? NId)
        {
            var NontiData = _context.NotificationRecords.FirstOrDefault(n => n.ActivityId == NId);
            var loggedInUserName = User.Identity.Name;

            // 檢查 loggedInUserName 是否重複
            var existingOrder = _context.BookingOrders
                .FirstOrDefault(b => b.ActivityId == NId && b.UserName == loggedInUserName);

            if (existingOrder != null)
            {
                return (null, BookingResult.UserAlreadyBooked);
            }

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
                    return (null, BookingResult.ActivityFull);
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

            return (newBookingOrder, BookingResult.Success);
        }

        // 寫入排隊
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





        //----------------------------------------------------------------------------------------
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

        //OrganizerActiveCheck使用
        // POST: /Confirm/QRCheck
        [HttpPost]
        public async Task<IActionResult> QRCheck([FromBody] QRRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Barcode))
            {
                return BadRequest("資料錯誤");
            }
            string userName = request.userName;
            string barcode = request.Barcode;
            int activityId = request.activityId;


            var aa = _context.BookingOrders.FirstOrDefault(
                    e => e.UserName == userName && e.ActivityBarcode == barcode && e.ActivityId == activityId
                );

            if (aa == null)
            {
                return NotFound(new { success = false, message = "找不到對應的簽到資訊" });
            }

            if (aa.Checked == false)
            {
                aa.Checked = true;
            }
            else {
                return Ok(new { success = false, message = "有簽到過了喔~" });
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "簽到成功" });
        }

        //建立一個QRRequest給QRCheck用
        public class QRRequest
        {
            public string userName { get; set; }
            public string Barcode { get; set; }
            public int activityId { get; set; }
        }

        //--------------------------------------------------------

        //管理者使用 簽到 -  Admin那邊用的
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




        //---------------------------
        //UserAcitvityvVew
        //一般會員瀏覽歷史紀錄參加活動的頁面
        //GET:/Confirm/UserAcitvityvVew
        [HttpGet]
        public IActionResult UserAcitvityvVew()
        {
            return View();
        }



        // UserActivityView用的
        // 呼叫會員參加的資料api
        // GET:/Confirm/JoinList
        [HttpGet]
        public async Task<JsonResult> JoinList()
        {
            // 獲取當前登入的使用者
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(null);
            }

            var userName = user.UserName; // 直接從 user 對象中獲取 UserName
            var userId = user.Id; // 直接從 user 對象中獲取 UserId

            var results = (from bo in _context.BookingOrders
                           join nr in _context.NotificationRecords on bo.ActivityId equals nr.ActivityId
                           where bo.UserName == userName
                                 && nr.EndTime > DateTime.Now
                                 && nr.IsActivity == true
                           orderby nr.EndTime, bo.ActivityId, bo.WaitingNumber
                           select new
                           {
                               bo.ActivityId,
                               bo.UserName,
                               bo.WaitingNumber,
                               nr.ActivityName,
                               nr.ActivityPhoto,
                               nr.StartTime,
                               nr.EndTime
                           }).ToList();

            return Json(results);
        }

        // UserActivityView用的
        // 呼叫會員參加的資料api
        // GET:/Confirm/historyList
        [HttpGet]
        public async Task<JsonResult> historyList()
        {
            // 獲取當前登入的使用者
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(null);
            }

            var userName = user.UserName; // 直接從 user 對象中獲取 UserName
            var userId = user.Id; // 直接從 user 對象中獲取 UserId

            var results = (from bo in _context.BookingOrders
                           join nr in _context.NotificationRecords on bo.ActivityId equals nr.ActivityId
                           where bo.UserName == userName
                                 && nr.EndTime < DateTime.Now
                           orderby bo.ActivityId, bo.WaitingNumber
                           select new
                           {
                               bo.ActivityId,
                               bo.UserName,
                               bo.WaitingNumber,
                               bo.Checked,
                               nr.ActivityName,
                               nr.ActivityPhoto,
                               nr.StartTime,
                               nr.EndTime
                           }).ToList();

            return Json(results);
        }

        //--------------------------------------------------------------------------


        //BookOK
        //使用者加入成功的頁面
        //GET:/Confirm/BookOK
        [HttpGet]
        public IActionResult BookOK()
        { 
            return View();
        }




        // BookOK用的
        // 參加資料的api
        // GET:/Confirm/BookOKList
        [HttpGet]
        public async Task<JsonResult> BookOKList(int id)
        {
            // 獲取當前登入的使用者
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(null);
            }

            var userName = user.UserName; // 直接從 user 對象中獲取 UserName
            var userId = user.Id; // 直接從 user 對象中獲取 UserId

            var result = (from bo in _context.BookingOrders
                          join nr in _context.NotificationRecords on bo.ActivityId equals nr.ActivityId
                          where bo.UserName == userName
                                && bo.ActivityId == id
                          select new
                          { 
                              bo.ActivityBarcode,
                              bo.ActivityId,
                              bo.UserName,
                              bo.WaitingNumber,
                              bo.Checked,
                              nr.ActivityName,
                              nr.ActivityPhoto,
                              nr.StartTime,
                              nr.EndTime
                          }).FirstOrDefault();

            return Json(result);
        }




        //QRcode判斷
        //POST: //Confirm/CheckIn
        [HttpPost]
        public async Task<JsonResult> CheckIn(int activityId, string randomCode)
        {
            // 獲取當前登入的使用者
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(null);
            }

            var userName = user.UserName; // 直接從 user 對象中獲取 UserName
            var userId = user.Id; // 直接從 user 對象中獲取 UserId


            var activity =_context.BookingOrders.FirstOrDefault(a =>
                a.ActivityId == activityId &&
                a.UserName == userName &&
                a.ActivityBarcode == randomCode);


            if (activity != null)
            {
                activity.Checked = true;
                return Json(new { success = true, message = "Checked in successfully." });
            }

            else
            {
                return Json(new { success = false, message = "Invalid credentials." });
            }

        }




        //----------------------------------------------------------------------------
        //GET:/Confirm/NumberFull
        [HttpGet]
        public IActionResult NumberFull()
        {
            return View();
        }


    }
}
