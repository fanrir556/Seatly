using static Seatly1.Models.MailSetting;

namespace Seatly1.Helper
{
    public interface IMailService
    {
        Task SendEmailiAsync(MailRequest mailRequest);
    }
}
