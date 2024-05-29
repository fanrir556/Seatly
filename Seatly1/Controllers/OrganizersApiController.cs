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
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Microsoft.AspNetCore.Hosting;
using System.Collections;

namespace Seatly1.Controllers
{
    // 活動方資訊頁的api
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizersApiController : ControllerBase
    {
        private readonly SeatlyContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrganizersApiController(SeatlyContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 列出個別活動方的所有活動
        [HttpGet("activities/{organizerId}")]
        public async Task<IEnumerable<NotificationRecordDTO>> GetActivitiesForOrganizerByPage(int organizerId, int page)
        {
            const int pageSize = 50; // 每頁顯示的資料筆數

            var offset = (page - 1) * pageSize; // 計算偏移量

            var activities = await _context.NotificationRecords
                .Where(activity => activity.OrganizerId == organizerId)
                .OrderBy(activity => activity.ActivityId) // 如果有一個唯一的 ID，可以用它來排序，否則用其他適合的方式排序
                .Skip(offset) // 跳過偏移量
                .Take(pageSize) // 取得指定頁數的資料
                .Select(activity => new NotificationRecordDTO
                {
                    ActivityId = activity.ActivityId,
                    ActivityPhoto = activity.ActivityPhoto,
                    StartTime = activity.StartTime,
                    EndTime = activity.EndTime,
                    ActivityName = activity.ActivityName,
                    HashTag1 = activity.HashTag1,
                    HashTag2 = activity.HashTag2,
                    HashTag3 = activity.HashTag3,
                    HashTag4 = activity.HashTag4,
                    HashTag5 = activity.HashTag5,
                })
                .ToListAsync();

            return activities;
        }
        // 根據活動id取得活動資訊
        [HttpGet("activity/{activityid}")]
        public async Task<NotificationRecordDTO?> GetActivity(int activityid)
        {
            var activity = await _context.NotificationRecords.FindAsync(activityid);
            if (activity == null)
            {
                return null;
            }
            NotificationRecordDTO activityDTO = new()
            {
                ActivityId = activity.ActivityId,
                ActivityPhoto = activity.ActivityPhoto,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                Capacity = activity.Capacity,    
                ActivityName = activity.ActivityName,
                ActivityMethod = activity.ActivityMethod,
                DescriptionN = activity.DescriptionN,
                HashTag1 = activity.HashTag1,
                HashTag2 = activity.HashTag2,
                HashTag3 = activity.HashTag3,
                HashTag4 = activity.HashTag4,
                HashTag5 = activity.HashTag5,
                Location = activity.Location,
                LocationDescription = activity.LocationDescription,
            };
            return activityDTO;
        }

        [HttpPost("activity")]
        public async Task<string> PostActivitiesForOrganizer(NotificationRecordDTO2 activity)
        {
            if (activity.ActivityPhoto == null)
            {
                return "未提供活動照片";
            }

            //MemoryStream 用於將 IFormFile 讀取為 byte 陣列，然後將 byte 陣列儲存到 varbinary 欄位。
            using var memoryStream = new MemoryStream();
            await activity.ActivityPhoto.CopyToAsync(memoryStream);
            var photoBytes = memoryStream.ToArray();

            NotificationRecord act = new()
            {
                ActivityId = activity.ActivityId,
                OrganizerId = activity.OrganizerId,
                ActivityPhoto = photoBytes,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                Capacity = activity.Capacity,
                ActivityName = activity.ActivityName,
                ActivityMethod = activity.ActivityMethod,
                IsActivity = activity.IsActivity,
                Location = activity.Location,
                LocationDescription = activity.LocationDescription,
                HashTag1 = activity.HashTag1,
                HashTag2 = activity.HashTag2,
                HashTag3 = activity.HashTag3,
                HashTag4 = activity.HashTag4,
                HashTag5 = activity.HashTag5,
            };

            _context.NotificationRecords.Add(act);
            await _context.SaveChangesAsync();
            return $"{act.ActivityId}";
        }

        [HttpPut("activity/{id}")]
        public async Task<string> PutActivitiesForOrganizer(int id, NotificationRecordDTO2 activity)
        {
            var existingActivity = await _context.NotificationRecords.FindAsync(id);
            if (existingActivity == null)
            {
                return "活動不存在";
            }

            if (activity.ActivityPhoto == null)
            {
                return "未提供活動照片";
            }

            // 將 IFormFile 讀取為 byte 陣列，然後將 byte 陣列儲存到 varbinary 欄位。
            using var memoryStream = new MemoryStream();
            await activity.ActivityPhoto.CopyToAsync(memoryStream);
            var photoBytes = memoryStream.ToArray();

            // 更新現有活動的屬性
            existingActivity.OrganizerId = activity.OrganizerId;
            existingActivity.ActivityPhoto = photoBytes;
            existingActivity.StartTime = activity.StartTime;
            existingActivity.EndTime = activity.EndTime;
            existingActivity.Capacity = activity.Capacity;
            existingActivity.ActivityName = activity.ActivityName;
            existingActivity.ActivityMethod = activity.ActivityMethod;
            existingActivity.IsActivity = activity.IsActivity;
            existingActivity.Location = activity.Location;
            existingActivity.LocationDescription = activity.LocationDescription;
            existingActivity.HashTag1 = activity.HashTag1;
            existingActivity.HashTag2 = activity.HashTag2;
            existingActivity.HashTag3 = activity.HashTag3;
            existingActivity.HashTag4 = activity.HashTag4;
            existingActivity.HashTag5 = activity.HashTag5;

            // 儲存變更
            await _context.SaveChangesAsync();

            return $"{existingActivity.ActivityId}";
        }

        // 修改活動敘述
        [HttpPatch("activity/{id}")]
        public async Task<string> PatchActivityDescription(int id, ActivityDescriptionDTO activity)
        {
            var existingActivity = await _context.NotificationRecords.FindAsync(id);
            if (existingActivity == null)
            {
                return "活動不存在";
            }

            // 更新現有活動的屬性
            existingActivity.DescriptionN = activity.DescriptionN;

            // 儲存變更
            await _context.SaveChangesAsync();

            return "修改描述成功";
        }

        // 取的活動敘述
        [HttpGet("activity/description/{id}")]
        public async Task<ActivityDescriptionDTO?> GetActivityDescription(int id)
        {
            var activity = await _context.NotificationRecords.FindAsync(id);
            if (activity == null)
            {
                return null;
            }
            ActivityDescriptionDTO activityDTO = new()
            {
                DescriptionN = activity.DescriptionN,
            };
            return activityDTO;
        }

        // 刪除活動
        [HttpDelete("activity/{id}")]
        public async Task<string> DeleteActivity(int id)
        {
            var activity = await _context.NotificationRecords.FindAsync(id);
            if(activity == null)
            {
                return "找不到要刪除的活動";
            }
            try
            {
                _context.NotificationRecords.Remove(activity);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                return "刪除失敗";
            }
            return "刪除成功";
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

        // 讀取cookie驗證是否登入
        [HttpGet("cookie")]
        public IActionResult CheckLoginStatus()
        {
            // 获取请求中的 cookie 数据
            string? cookieValue = Request.Cookies["OrganizerId"];

            // 检查是否存在有效的登录会话或认证凭据
            if (cookieValue != null)
            {
                // 用户已登录，返回成功的响应
                return Ok(cookieValue);
            }
            else
            {
                // 用户未登录，返回错误的响应
                return Unauthorized("活動方未登入");
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
            if (await _context.Organizers.AnyAsync(o => o.OrganizerAccount == organizer.OrganizerAccount))
            {
                return BadRequest("帳號已被註冊");
            }
            else if (await _context.Organizers.AnyAsync(o => o.Email == organizer.Email))
            {
                return BadRequest("信箱已被註冊");
            }
            else if (await _context.Organizers.AnyAsync(o => o.Phone == organizer.Phone))
            {
                return BadRequest("電話已被註冊");
            }
            else
            {
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
        }

        // 讀取活動方照片
        [HttpGet("photo/{organizerid}")]
        public async Task<IActionResult> GetOrganizerPhoto(int organizerid, string FileName)
        {
            string WebRootPath = _webHostEnvironment.WebRootPath;
            var organizer = await _context.Organizers.FindAsync(organizerid);

            if (organizer == null)
            {
                return NotFound();
            }

            var photoPath = Path.Combine(WebRootPath, "uploads/organizers", FileName);

            var imageFileStream = System.IO.File.OpenRead(photoPath);

            string fileExtension = Path.GetExtension(imageFileStream.Name).ToLower();
            return File(imageFileStream, $"uploads/{fileExtension}");
        }

        // 讀取活動圖片，資料庫存圖片名稱不直接存圖片本身的方法
        //[HttpGet("activityPhoto/{activityid}")]
        //public async Task<IActionResult> GetActivityPhoto(int activityid, string FileName)
        //{
        //    string WebRootPath = _webHostEnvironment.WebRootPath;
        //    var activity = await _context.NotificationRecords.FindAsync(activityid);

        //    if (activity == null)
        //    {
        //        return NotFound();
        //    }

        //    var photoPath = Path.Combine(WebRootPath, "uploads", FileName);

        //    var imageFileStream = System.IO.File.OpenRead(photoPath);

        //    string fileExtension = Path.GetExtension(imageFileStream.Name).ToLower();
        //    return File(imageFileStream, $"uploads/{fileExtension}");
        //}

        // 註冊活動方圖片上傳
        [HttpPost("uploads")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            // 处理图片上传逻辑，例如保存到服务器上的某个位置
            // 这里只是一个简单的示例，将图片保存到 wwwroot/uploads 文件夹下
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/organizers");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = image.FileName; // 保留原始檔名
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }

        // 活動描述圖片上傳
        [HttpPost("uploads/description")]
        public async Task<IActionResult> descriptionImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            // 处理图片上传逻辑，例如保存到服务器上的某个位置
            // 这里只是一个简单的示例，将图片保存到 wwwroot/uploads 文件夹下
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/description");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = image.FileName; // 保留原始檔名
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }

        // 刪除活動方照片
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteImage(string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    await Task.Run(() => System.IO.File.Delete(filePath));
                    return Ok($"File {fileName} deleted successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting file: {ex.Message}");
                }
            }
            else
            {
                return NotFound($"File {fileName} not found.");
            }
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 清除用户的身份验证凭证（例如，清除存储在 cookie 中的身份验证令牌）
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            option.HttpOnly = true;
            option.Secure = true;
            Response.Cookies.Append("OrganizerId", "", option);
            return Ok("登出成功");
            // 返回成功响应
        }

        // 修改活動方資訊
        [HttpPut("put/{id}")]
        public async Task<ActionResult<Organizers>> PutOrganizer(int id, OrganizerDTO organizer)
        {
            var org = await _context.Organizers.FindAsync(id);
            if (org == null)
            {
                return NotFound("活動方不存在");
            }
            else if (await _context.Organizers.AnyAsync(o => o.Email == organizer.Email && o.OrganizerId != id))
            {
                return BadRequest("信箱已被註冊");
            }
            else if (await _context.Organizers.AnyAsync(o => o.Phone == organizer.Phone && o.OrganizerId != id))
            {
                return BadRequest("電話已被註冊");
            }
            else
            {
                org.LoginPassword = organizer.LoginPassword;
                org.OrganizerName = organizer.OrganizerName;
                org.OrganizerCategory = organizer.OrganizerCategory;
                org.OrganizerPhoto = organizer.OrganizerPhoto;
                org.Menu = organizer.Menu;
                org.Address = organizer.Address;
                org.ReservationUrl = organizer.ReservationUrl;
                org.Hashtag = organizer.Hashtag;
                org.Email = organizer.Email;
                org.Phone = organizer.Phone;
                _context.Entry(org).State = EntityState.Modified;
                try { await _context.AddRangeAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                await _context.SaveChangesAsync();
                return Ok("已送出修改");
            }
        }

    }
}
