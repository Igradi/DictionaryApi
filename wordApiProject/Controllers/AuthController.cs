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
            try
            {
                var primaryKey = from User in _context.Users where (User.Email == LoginUser.UserName) select User.Id;
                int id = primaryKey.First();
                var logedInUser = _context.Users.Find(id);

                if (!(BCrypt.Net.BCrypt.Verify(LoginUser.Password, logedInUser.Password)))
                {
                    logedInUser.FailedPasswordAttempts++;
                    _context.SaveChangesAsync();
                    return BadRequest("Wrong password");
                }
                else if (logedInUser.FailedPasswordAttempts >= 3)
                {
                    return BadRequest("you are banned");
                }
                else
                {
                    ResetFailedAttempts(logedInUser);
                    var claims = new[]
                    {
                        new Claim(type: "id",value: logedInUser.Id.ToString()),
                        new Claim(type: "role",value:logedInUser.role.ToString()),
                    };
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication@345"));
                    var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "https://localhost:7153",
                        audience: "https://localhost:7153",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signingCredentials
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);


                    return Ok(new { Token = tokenString });
                }
            }
            catch
            {
                return BadRequest("User not found");
            }
        }
        private void ResetFailedAttempts(User logedInUser)
        {
            logedInUser.FailedPasswordAttempts = 0;
            _context.SaveChangesAsync();
        }
    }
}