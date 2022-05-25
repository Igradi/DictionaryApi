using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<User>> getUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest();
            return Ok( user);
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
        public async Task<ActionResult<User>> ChangeUser(User newUser)
        {
            var oldUser = await _context.Users.FindAsync(newUser.Id);
            oldUser.Name = String.Copy(newUser.Name); ;
            oldUser.LastName = String.Copy(newUser.LastName);
            oldUser.DateOfBirth = String.Copy(newUser.DateOfBirth);
            oldUser.Email = String.Copy(newUser.Email);
            oldUser.BussinessMail = String.Copy(newUser.BussinessMail);
            oldUser.Description = String.Copy(newUser.Description);
            await _context.SaveChangesAsync();
            return (oldUser);

            

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
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
