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
    public class OrganizersAdminController : Controller
    {
        private readonly SeatlyContext _context;

        public OrganizersAdminController(SeatlyContext context)
        {
            _context = context;
        }

        // GET: OrganizersAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Organizers.ToListAsync());
        }

        // GET: OrganizersAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizer = await _context.Organizers
                .FirstOrDefaultAsync(m => m.OrganizerId == id);
            if (organizer == null)
            {
                return NotFound();
            }

            return View(organizer);
        }

        // GET: OrganizersAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrganizersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizerId,OrganizerAccount,LoginPassword,OrganizerName,OrganizerCategory,OrganizerPhoto,Menu,Address,ReservationUrl,Hashtag,Email,Phone,Validation")] Organizer organizer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organizer);
                await _context.SaveChangesAsync();
                return View("~/Views/Admin/Index.cshtml");
            }
            return View(organizer);
        }

        // GET: OrganizersAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return NotFound();
            }
            return View(organizer);
        }

        // POST: OrganizersAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizerId,OrganizerAccount,LoginPassword,OrganizerName,OrganizerCategory,OrganizerPhoto,Menu,Address,ReservationUrl,Hashtag,Email,Phone,Validation")] Organizer organizer)
        {
            if (id != organizer.OrganizerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organizer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizerExists(organizer.OrganizerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("~/Views/Admin/Index.cshtml");
            }
            return View(organizer);
        }

        // GET: OrganizersAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizer = await _context.Organizers
                .FirstOrDefaultAsync(m => m.OrganizerId == id);
            if (organizer == null)
            {
                return NotFound();
            }

            return View(organizer);
        }

        // POST: OrganizersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer != null)
            {
                _context.Organizers.Remove(organizer);
            }

            await _context.SaveChangesAsync();
            return View("~/Views/Admin/Index.cshtml");
        }

        private bool OrganizerExists(int id)
        {
            return _context.Organizers.Any(e => e.OrganizerId == id);
        }
    }
}
