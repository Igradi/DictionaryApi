using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using wordApiProject.Models;

namespace wordApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail(string body)
        {
            var resetUser = new ResetPasswordRequest();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("aglae.hackett58@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("aglae.hackett58@ethereal.email"));
            email.Subject = "test mail subject";
            email.Body = new TextPart(TextFormat.Html) { Text = resetUser.Token };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("aglae.hackett58@ethereal.email", "tJ6m3h6BXhAdzJ9YuF");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
