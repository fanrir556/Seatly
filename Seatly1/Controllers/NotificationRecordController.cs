using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Seatly1.Models;
using Seatly1.ViewModels;
using System.Web;

namespace Seatly1.Controllers
{
    public class NotificationRecordController : Controller
    {
        private readonly SeatlyContext _context;

        public NotificationRecordController(SeatlyContext context)
        {
            _context = context;
        }

        // GET: NotificationRecord
        public async Task<IActionResult> Index()
        {
            return View(await _context.NotificationRecords.ToListAsync());
        }

        // GET: NotificationRecord/_Details/
        //_Details部分檢視
        public async Task<IActionResult> _Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationRecord = await _context.NotificationRecords
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (notificationRecord == null)
            {
                return NotFound();
            }

            return PartialView("~/Views/NotificationRecord/Details.cshtml", notificationRecord);
        }

        // GET: NotificationRecord/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationRecord = await _context.NotificationRecords
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (notificationRecord == null)
            {
                return NotFound();
            }

            return View(notificationRecord);
        }

        // GET: NotificationRecord/UsersView/
        public async Task<IActionResult> UsersView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new NotificationBookReader();

            var notificationRecord = await _context.NotificationRecords
                .FirstOrDefaultAsync(m => m.ActivityId == id);

            var Book = _context.BookingOrders
                .Where(b => b.ActivityId == id)
                .OrderBy(c => c.WaitingNumber)
                .LastOrDefault();

            viewModel.NotificationRecord = notificationRecord;
            viewModel.BookingOrder = Book;

            if (notificationRecord == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }


        // GET: NotificationRecord/Create
        public IActionResult Create()
        {
           
            return PartialView();
        }
     // GET: NotificationRecord/_Create
        public IActionResult _Create()
        {
            return PartialView("~/Views/NotificationRecord/Create.cshtml");
        }

        // POST: NotificationRecord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,OrganizerId,ActivityPhoto,StartTime,EndTime,Capacity,ActivityName,DescriptionN,IsRecurring,RecurringTime,ActivityMethod,IsActivity,HashTag1,HashTag2,HashTag3,HashTag4,HashTag5")]
        NotificationRecord notificationRecord)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files["ActivityPhoto"] != null) 
                {
                    SetPhoto(notificationRecord);
                }
                if (notificationRecord.IsActivity == null) {
                    notificationRecord.IsActivity = true;
                }



                _context.Add(notificationRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction("_NotificationRecord", "Admin");
            }
            return RedirectToAction("_NotificationRecord", "Admin");
        }

        



        // GET: NotificationRecord/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationRecord = await _context.NotificationRecords.FindAsync(id);
            if (notificationRecord == null)
            {
                return NotFound();
            }
            return PartialView("~/Views/NotificationRecord/Edit.cshtml", notificationRecord);
        }

        // POST: NotificationRecord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("ActivityId,OrganizerId,ActivityPhoto,StartTime,EndTime,Capacity,ActivityName,DescriptionN,IsRecurring,RecurringTime,ActivityMethod,Location,IsActivity,HashTag1,HashTag2,HashTag3,HashTag4,HashTag5")] NotificationRecord notificationRecord)

        {
            NotificationRecord nr = await _context.NotificationRecords.FindAsync(notificationRecord.ActivityId);

            if (notificationRecord.ActivityId != nr.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    NotificationRecord c = await _context.NotificationRecords.FindAsync(notificationRecord.ActivityId);
                    if (Request.Form.Files["ActivityPhoto"] != null)
                    {
                       await SetPhoto(notificationRecord);
                    }
                    else {
                        notificationRecord.ActivityPhoto = c.ActivityPhoto;
                    }

                    _context.Entry(c).State = EntityState.Detached;
                    _context.Update(notificationRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationRecordExists(notificationRecord.ActivityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("_NotificationRecord", "Admin");
            }
            return PartialView(notificationRecord);
        }

        private Task <bool> SetPhoto(NotificationRecord notificationRecord)
        {
            using (BinaryReader br = new
            BinaryReader(Request.Form.Files["ActivityPhoto"].OpenReadStream())) //BinaryReader一次性
            {
                notificationRecord.ActivityPhoto = br.ReadBytes((int)Request.Form.Files["ActivityPhoto"].Length);
            }

            return Task.FromResult(true);
        }

        // GET: NotificationRecord/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationRecord = await _context.NotificationRecords
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (notificationRecord == null)
            {
                return NotFound();
            }

            return View(notificationRecord);
        }

        // POST: NotificationRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notificationRecord = await _context.NotificationRecords.FindAsync(id);
            if (notificationRecord != null)
            {
                _context.NotificationRecords.Remove(notificationRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(/*nameof(Index)*/);
        }



        //GET: NotificationRecord/GetActivityPhoto/(id)
        public async Task<FileResult> GetActivityPhoto(int? id)
        {
            NotificationRecord C = await _context.NotificationRecords.FindAsync(id);
            byte[]? content = C?.ActivityPhoto;
            return File(content, "image/jpeg");
        }


        // UserView 呼叫列表用
        // GET: NotificationRecord/UserList
        [HttpGet]
        public async Task<IActionResult> UserList(int id)
        {
            // 使用異步方法來獲取數據並轉換為列表
            var record = await _context.NotificationRecords
                                        .Where(e => e.ActivityId == id)
                                        .FirstOrDefaultAsync();

            // 檢查是否有結果，並返回相應的響應
            if (record == null)
            {
                return NotFound(); // 或其他適當的狀態碼
            }

            // 返回 JSON 格式的數據
            return Ok(record);
        }

        private bool NotificationRecordExists(int id)
        {
            return _context.NotificationRecords.Any(e => e.ActivityId == id);
        }
    }
}
