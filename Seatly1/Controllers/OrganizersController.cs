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
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizersController : ControllerBase
    {
        private readonly SeatlyContext _context;

        public OrganizersController(SeatlyContext context)
        {
            _context = context;
        }

        // POST: api/Organizers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<IActionResult> Login(OrganizerLoginDTO organizerloginDTO)
        {
            var user = await _context.Organizers.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized(); // Invalid credentials
            }

            // Generate and return an access token (e.g., using JWT)
            // ...

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(OrganizerLoginDTO organizerloginDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                return BadRequest("Username already exists");
            }

            var user = new User
            {
                Username = model.Username,
                // Hash and salt the password
                // ...
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        //Helper method to verify password hash
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            // ...
        }
    }
}
