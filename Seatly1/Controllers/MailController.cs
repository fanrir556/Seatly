using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seatly1.Helper;
using static Seatly1.Models.MailSetting;

namespace Seatly1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        // 套用Mail的Method
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        // 活動方註冊、修改信箱驗證信發送
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)// 這裡因為有檔案所以要使用`[FromForm]`
        {
            try
            {
                await _mailService.SendEmailiAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
