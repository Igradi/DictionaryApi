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