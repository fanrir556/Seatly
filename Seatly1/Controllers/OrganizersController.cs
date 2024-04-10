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
        [HttpGet("infoes")]
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

        // 讀取cookie驗證活動方式否登入
        [HttpGet("cookie")]
        public IActionResult CheckLoginStatus()
        {
            // 获取请求中的 cookie 数据
            var cookieValue = Request.Cookies["OrganizerId"];

            // 检查是否存在有效的登录会话或认证凭据
            if (cookieValue != null)
            {
                // 用户已登录，返回成功的响应
                return Ok($"活動方已登入，id是 {cookieValue}");
            }
            else
            {
                // 用户未登录，返回错误的响应
                return Unauthorized("User is not logged in.");
            }
        }

        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<ActionResult<Organizers>> Login(OrganizerLoginDTO orglogindto)
        {
            var user = await _context.Organizers.FirstOrDefaultAsync(u => u.OrganizerAccount == orglogindto.OrganizerAccount && u.LoginPassword == orglogindto.LoginPassword);

            if (user == null)
            {
                return Unauthorized("帳號或密碼錯誤");
            }
            else if (user != null && user.Validation == false)
            {
                return Unauthorized("該活動方未完成審核");
            }
            else
            {
                // 将用户的唯一标识符添加到Cookie中
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);
                option.HttpOnly = true;
                option.Secure = true;
                Response.Cookies.Append("OrganizerId",user.OrganizerId.ToString(), option);
                return Ok(user);
            }
        }

        // 註冊的文字上傳api
        [HttpPost("register")]
        public async Task<ActionResult<Organizers>> RegisterOrganizer(OrganizerDTO organizer)
        {
            // 检查邮箱、电子邮件和电话号码是否已被注册
            if (await _context.Organizers.AnyAsync(o =>
                o.OrganizerAccount == organizer.OrganizerAccount ||
                o.Email == organizer.Email ||
                o.Phone == organizer.Phone))
            {
                return BadRequest("帳號、Email 或電話已經被註冊過了。");
            }

            Organizer o = new()
                {
                    OrganizerAccount = organizer.OrganizerAccount,
                    LoginPassword = organizer.LoginPassword,
                    OrganizerName = organizer.OrganizerName,
                    OrganizerCategory = organizer.OrganizerCategory,
                    OrganizerPhoto = organizer.OrganizerPhoto,
                    Menu = organizer.Menu,
                    Address = organizer.Address,
                    ReservationUrl = organizer.ReservationUrl,
                    Hashtag = organizer.Hashtag,
                    Email = organizer.Email,
                    Phone = organizer.Phone,
                    Validation = organizer.Validation
                };
                _context.Organizers.Add(o);
                await _context.SaveChangesAsync();
                return Ok("已完成註冊，待審核完成後即可登入");
        }

        // 註冊的圖片上傳api
        [HttpPost("uploads")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            // 处理图片上传逻辑，例如保存到服务器上的某个位置
            // 这里只是一个简单的示例，将图片保存到 wwwroot/uploads 文件夹下
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 清除用户的身份验证凭证（例如，清除存储在 cookie 中的身份验证令牌）
            Response.Cookies.Delete("OrganizerId");

            // 返回成功响应
            return Ok("登出成功");
        }
    }
}
