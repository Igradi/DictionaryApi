using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace wordApiProject
{
   /* [Route("api/[controller]")]
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
            return Ok(await _context.Wordss.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Words>> Get(int id)
        {
            var word = await _context.Wordss.FindAsync(id);
            if (word == null)
                return BadRequest("word not found.");
            return Ok(word);
        }

        [HttpPost]
        public async Task<ActionResult<List<Words>>> AddWord(Words word)
        {
            _context.Wordss.Add(word);
            await _context.SaveChangesAsync();

            return Ok(await _context.Wordss.ToListAsync());
        }
        [HttpPut]
        public async Task<ActionResult<List<Words>>> UpdateWord(Words request)
        {
            var dbWord = await _context.Wordss.FindAsync(request.Id);
            if (dbWord == null)
                return BadRequest("word not found.");

            dbWord.WordName = request.WordName;
            dbWord.HasId = request.HasId;

            await _context.SaveChangesAsync();

            return Ok(await _context.Wordss.ToListAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Words>>> Delete(int id)
        {
            var dbWord = await _context.Wordss.FindAsync(id);
            if (dbWord == null)
                return BadRequest("word not found.");

            _context.Wordss.Remove(dbWord);
            await _context.SaveChangesAsync();

            return Ok(await _context.Wordss.ToListAsync());
        }
    }*/
}