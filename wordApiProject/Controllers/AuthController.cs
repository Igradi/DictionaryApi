using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wordApiProject.Models;
namespace wordApiProject.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        public AuthController(DataContext context)
        {
            _context = context;
        }
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginModel LoginUser)
        {
            var checkpassword = string.Empty;
            var primaryKey = from User in _context.Users where (User.Email == LoginUser.UserName) select User.Id;
            int id = primaryKey.First();
            var check = _context.Users.Find(id);

            if (check == null)
            {
                return BadRequest("invalid client request");
            }
                checkpassword = check.Password;
            
            //Console.WriteLine(LoginUser.UserName);
            //Console.WriteLine(LoginUser.Password);
            if(check.Password != checkpassword)
            {
                return BadRequest("invalid client request");
            }
            else
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication@345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7153",
                    audience: "https://localhost:7153",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials
                    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            //return Unauthorized();
        }
    }
}