using Microsoft.AspNetCore.Mvc;
using wordApiProject.Models;

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
        public async Task<ActionResult<List<User>>> getUsers()
        {
            return Ok(await _context.Users.ToListAsync());
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
            user.BussinessMail = editUser.BusinessMail;
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
        
    }
}
