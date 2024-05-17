
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Seatly1.Areas.Identity
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress("queuely139@outlook.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlMessage;

            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
            //SmtpClient client = new SmtpClient("smtp.live.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("queuely139@outlook.com", "P@ssw0rd139");
            client.EnableSsl = true;
            client.Send(mail);
        }
    }
}