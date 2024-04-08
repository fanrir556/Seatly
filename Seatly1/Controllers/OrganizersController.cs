using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Seatly1.DTO;
using Seatly1.Models;
using System.Diagnostics;

namespace Seatly1.Controllers
{
    // 活動方資訊頁的api
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizersController : ControllerBase
    {
        private readonly SeatlyContext _context;

        public OrganizersController(SeatlyContext context)
        {
            _context = context;
        }

        // 活動方資訊取得api
        [HttpGet("totalinfo")]
        public async Task<IEnumerable<OrgainzerInfoDTO>> GetOrganizers()
        {
            return await _context.Organizers
                .Select(org => new OrgainzerInfoDTO
                { 
                    OrganizerAccount = org.OrganizerAccount,
                    OrganizerName = org.OrganizerName,
                    OrganizerCategory = org.OrganizerCategory,
                    OrganizerPhoto = org.OrganizerPhoto,
                    Menu = org.Menu,
                    Address = org.Address,
                    ReservationUrl = org.ReservationUrl,
                    Hashtag = org.Hashtag,
                    Email = org.Email,
                    Phone = org.Phone,
                })
                .ToListAsync();
        }

        // 活動方個別會員資訊取得api
        [HttpGet("info/{id}")]
        public async Task<OrgainzerInfoDTO?> GetOrganizer(int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
            {
                return null;
            }
            OrgainzerInfoDTO orgInfo = new OrgainzerInfoDTO
            {
                OrganizerAccount = organizer.OrganizerAccount,
                OrganizerName = organizer.OrganizerName,
                OrganizerCategory = organizer.OrganizerCategory,
                OrganizerPhoto = organizer.OrganizerPhoto,
                Menu = organizer.Menu,
                Address = organizer.Address,
                ReservationUrl = organizer.ReservationUrl,
                Hashtag = organizer.Hashtag,
                Email = organizer.Email,
                Phone = organizer.Phone,
            };
            return orgInfo;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrganizer(int id, Organizer organizer)
        //{
        //    if (id != organizer.OrganizerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(organizer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrganizerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        ////To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public ActionResult Login(FormCollection post)
        //{
        //    string account = post["account"];
        //    string password = post["password"];

        //    //驗證密碼
        //    if (db.CheckUserData(account, password))
        //    {
        //        Response.Redirect("~/Home/Home");
        //        return new EmptyResult();
        //    }
        //    else
        //    {
        //        ViewBag.Msg = "登入失敗...";
        //        return View();
        //    }
        //}
        //[HttpPost("register")]
        //public async Task<string> RegisterOrganizer(OrganizerLoginDTO organizer)
        //{

        //    _context.Organizers.Add(organizer);
        //    await _context.SaveChangesAsync();

        //    return "註冊成功";
        //}

        //private bool OrganizerExists(int id)
        //{
        //    return _context.Organizers.Any(e => e.OrganizerId == id);
        //}
    }
}
