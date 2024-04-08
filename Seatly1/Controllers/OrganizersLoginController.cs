using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seatly1.DTO;
using Seatly1.Models;

namespace Seatly1.Controllers
{
    // 活動方登入的api
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizersLoginController : ControllerBase
    {
        private readonly SeatlyContext _context;

        public OrganizersLoginController(SeatlyContext context)
        {
            _context = context;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(OrganizerLoginDTO organizerloginDTO)
        //{
        //    var account = await _context.Organizers.FirstOrDefaultAsync(u => u.Username == model.Username);
        //    if (account == null || !VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
        //    {
        //        return Unauthorized(); // Invalid credentials
        //    }

        //    Generate and return an access token(e.g., using JWT)
        //     ...

        //    return Ok(new { message = "Login successful" });
        //}

        ////Helper method to verify password hash
        //private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        //{
        //    ...
        //}
    }
}
