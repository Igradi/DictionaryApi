using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace wordApiProject
{
   [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {

        private readonly DataContext _context;

        public WordsController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("{userID},{word},{wordType}")]
        public async Task<ActionResult> PutNewWord(int userID, string word, string wordType)
        {
            var newWord = new Words();
            var newHas = new Has();
            
            newWord.WordName = word;
            newWord.WordType = wordType;
            
             _context.Words.Add(newWord);
            await _context.SaveChangesAsync();

            var IdQuery = from Words in _context.Words where (Words.WordName == newWord.WordName) select Words.Id;
            
            newHas.WordId =IdQuery.First();
            newHas.UserId = userID;

            _context.Hass.Add(newHas);
            await _context.SaveChangesAsync();

            var IdHasQuery = from Hass in _context.Hass where ( Hass.WordId == newHas.WordId) select Hass.Id;
            var dbWord = await _context.Words.FindAsync(newHas.WordId);
            dbWord.HasId = IdHasQuery.First();
            await _context.SaveChangesAsync();
            return Ok(newWord);
        }

        [HttpGet]
        public async Task<ActionResult<List<Words>>> Get()
        {
            return Ok(await _context.Words.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Words>> Get(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word == null)
                return BadRequest("word not found.");
            return Ok(word);
        }

        [HttpPost]
        public async Task<ActionResult<List<Words>>> AddWord(Words word)
        {
            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return Ok(await _context.Words.ToListAsync());
        }
        [HttpPut]
        public async Task<ActionResult<List<Words>>> UpdateWord(Words request)
        {
            var dbWord = await _context.Words.FindAsync(request.Id);
            if (dbWord == null)
                return BadRequest("word not found.");

            dbWord.WordName = request.WordName;
            dbWord.HasId = request.HasId;

            await _context.SaveChangesAsync();

            return Ok(await _context.Words.ToListAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Words>>> Delete(int id)
        {
            var dbWord = await _context.Words.FindAsync(id);
            if (dbWord == null)
                return BadRequest("word not found.");

            _context.Words.Remove(dbWord);
            await _context.SaveChangesAsync();

            return Ok(await _context.Words.ToListAsync());
        }
    }
}