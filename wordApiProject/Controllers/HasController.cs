using Microsoft.AspNetCore.Mvc;

namespace wordApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HasController : ControllerBase
    {
        private readonly DataContext _context;

        public HasController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{USERID}")]
        public async  IAsyncEnumerable<List<Words>> GetWordsOfUSer(int USERID)
        {
            var HasIds = (from Has in _context.Hass where Has.UserId == USERID select Has.Id).ToList();
            var targetWords = (from Words in _context.Words where HasIds.Contains(Words.HasId) select Words).ToListAsync();


            yield return await targetWords;
        }
        [HttpDelete("id")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var user = _context.Users.Find(id);
            Console.WriteLine(user.Name);
            if (user == null)
            {
                return NotFound();
            }
            else
            _context.Users.Remove(user);
            _context.SaveChanges();
            var wordsId = (from Has in _context.Hass where Has.UserId == id select Has.WordId).ToList();
            var hassIds = from Has in _context.Hass where Has.UserId == id select Has;
            foreach(Has hass in hassIds)
            {
                _context.Hass.Remove(hass);
            }
            _context.SaveChanges();
            var words = from Words in _context.Words where wordsId.Contains(Words.Id) select Words;
            
            foreach (Words word in words)
            {
                _context.Words.Remove(word);
            }
            _context.SaveChanges();
            return Ok(id);
        }
    }
    }

