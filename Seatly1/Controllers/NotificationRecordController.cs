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

        // GET: NotificationRecord/Details/5
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

        // GET: NotificationRecord/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationRecord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,OrganizerId,ActivityPhoto,StartTime,EndTime,Capacity,ActivityName,DescriptionN,IsRecurring,RecurringTime")] NotificationRecord notificationRecord)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files["ActivityPhoto"] != null) 
                {
                    SetPhoto(notificationRecord);
                }


                _context.Add(notificationRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationRecord);
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
            return View(notificationRecord);
        }

        // POST: NotificationRecord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,OrganizerId,ActivityPhoto,StartTime,EndTime,Capacity,ActivityName,Description,IsRecurring,RecurringTime")] NotificationRecord notificationRecord)
        {
            if (id != notificationRecord.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    NotificationRecord c = await _context.NotificationRecords.FindAsync(id);
                    if (Request.Form.Files["ActivityPhoto"] != null)
                    {
                        SetPhoto(notificationRecord);
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
                return RedirectToAction(nameof(Index));
            }
            return View(notificationRecord);
        }

        private void SetPhoto(NotificationRecord notificationRecord)
        {
            using (BinaryReader br = new
            BinaryReader(Request.Form.Files["ActivityPhoto"].OpenReadStream())) //BinaryReader一次性
            {
                notificationRecord.ActivityPhoto = br.ReadBytes((int)Request.Form.Files["ActivityPhoto"].Length);
            }
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
            return RedirectToAction(nameof(Index));
        }


        //GET: NotificationRecord/GetActivityPhoto/(id)
        public async Task<FileResult> GetActivityPhoto(int? id)
        {
            NotificationRecord C = await _context.NotificationRecords.FindAsync(id);
            byte[]? content = C?.ActivityPhoto;
            return File(content, "image/jpeg");
        }



            private bool NotificationRecordExists(int id)
        {
            return _context.NotificationRecords.Any(e => e.ActivityId == id);
        }
    }
}
