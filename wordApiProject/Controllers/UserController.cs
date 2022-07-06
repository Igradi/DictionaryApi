using Microsoft.AspNetCore.Mvc;
using wordApiProject.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace wordApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<string>> getUsers()
        {
            

            var shortUsers = (from Users in _context.Users where Users.role != Role.admin select new {id =Users.Id, name= Users.Name,lastName = Users.LastName,email=Users.Email
            ,bussinessMail=Users.BussinessMail,nickname=Users.Nickname,phoneNumber = Users.PhoneNumber}).ToList();
            string jsonString = JsonConvert.SerializeObject(shortUsers);
           
            return jsonString;
        }
        [Route(("/api/[controller]/GETUSER"))]
        [HttpGet]
        public async Task<ActionResult<User>> getUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest();
            return Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> getUserNickame(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest();
            return Ok( user.Nickname);
        }
        [Route("/api/[controller]/POST")]
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User NewUser)
        {
            var dbUser = _context.Users.Where(u => u.Email == NewUser.Email).FirstOrDefault();
            if(dbUser != null)
            {
                return BadRequest("User already exist");
            }
            string changingPass = NewUser.Password;
          NewUser.Password = BCrypt.Net.BCrypt.HashPassword(changingPass);  
           await _context.Users.AddAsync(NewUser);
          await  _context.SaveChangesAsync();
            return Ok(NewUser);
        }

        [HttpPut]
        public async Task<ActionResult<User>> ChangeUser(int id,[FromBody] EditUserModel editUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return BadRequest();
            }
           
            user.Nickname = editUser.Nickname;
            user.BussinessMail = editUser.BussinessMail;
            user.PhoneNumber = editUser.ContactNumber;
            user.Description = editUser.Description;

            await _context.SaveChangesAsync();

            return Ok(await _context.Words.ToListAsync());

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest();
            }
             _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(10);
            await _context.SaveChangesAsync();

            var sentemail = new MimeMessage();
            sentemail.From.Add(MailboxAddress.Parse("lisette.keebler81@ethereal.email"));
            sentemail.To.Add(MailboxAddress.Parse("lisette.keebler81@ethereal.email"));
            sentemail.Subject = "test mail subject";
            sentemail.Body = new TextPart(TextFormat.Html) { Text = user.PasswordResetToken };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("lisette.keebler81@ethereal.email", "TS6vGNFkEs8TkP62WJ");
            smtp.Send(sentemail);
            smtp.Disconnect(true);

            return Ok("You may now reset your password.");
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResettPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }


            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password); 
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            user.FailedPasswordAttempts = 0;
            await _context.SaveChangesAsync();

            return Ok();
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(4));
        }
    }
}
