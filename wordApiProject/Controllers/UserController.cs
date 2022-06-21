using Microsoft.AspNetCore.Mvc;
using wordApiProject.Models;
using Newtonsoft.Json;
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
            

            var shortUsers = (from Users in _context.Users select new {id =Users.Id, name= Users.Name,lastName = Users.LastName,email=Users.Email
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
        
    }
}
