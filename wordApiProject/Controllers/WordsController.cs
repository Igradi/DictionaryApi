using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wordApiProject.Models;
using Newtonsoft.Json;

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

        [Route(("/api/[controller]/NumberOfwordsOfUsers"))]
        [HttpGet]
        public async Task<ActionResult> NumberOfWordsOfUsers()
        {
            AllWordsModel[] allWords = new AllWordsModel[4];

            for (int i = 0; i < 4; i++)
            {
                allWords[i] = new AllWordsModel();
            }

            var query = (from Words in _context.Words select Words).GroupBy(Words => Words.WordType).Select(y=> new { name = y.Key,value = y.Count()}).ToList();

            return Ok(query);
        }

        [HttpGet("{id},{wordName},{wordType}")]
        public async Task<ActionResult> PutNewWord(int id, string wordName, string wordType)
        {
            var newWord = new Words();
            var newHas = new UserWord();

            newWord.WordName = wordName;
            newWord.WordType = wordType;

            var wordExists = from Words in _context.Words where (Words.WordName == wordName) select Words.Id;

            if (wordExists.Count() > 0)
            {
                var link = from Has in _context.UserWords where (Has.WordId == wordExists.First()) select Has;
                bool userHasWord = await _context.UserWords.AnyAsync(x=>x.WordId == wordExists.First() && x.UserId==id);
                
                if (!userHasWord)
                {
                    newHas.WordId = link.First().WordId;
                    newHas.UserId = id;

                    _context.UserWords.Add(newHas);
                    await _context.SaveChangesAsync();

                }
            }
            else
            {
                _context.Words.Add(newWord);
                await _context.SaveChangesAsync();

                newHas.WordId = newWord.Id;
                newHas.UserId = id;

                _context.UserWords.Add(newHas);
                await _context.SaveChangesAsync();
               
                var dbWord = await _context.Words.FindAsync(newHas.WordId);
                dbWord.HasId = newHas.Id;
                await _context.SaveChangesAsync();
                
            }
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<Words>>> Get()
        {

            return Ok(await _context.Words.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetNumOfWords(int id)
        {
            int counter = 0;
            counter = _context.UserWords.Count(Hass => Hass.UserId == id);

            return (counter);
        }

        [Route(("/api/[controller]/getTypes"))]
        [HttpGet]
        public async Task<ActionResult<string>> GetNumOfTypes(int id)
        {
            TypesModel types = new TypesModel();

           var wordIds = (from UserWord in _context.UserWords where UserWord.UserId == id select UserWord.WordId).ToList();
            var query = (from Words in _context.Words where wordIds.Contains(Words.Id)select Words).GroupBy(x=>x.WordType).Select(y=> new { name = y.Key, value = y.Count() });
            
            foreach(var type in query)
            {
                switch (type.name)
                {
                    case "noun":
                        types.Nouns = type.value;
                        break;
                    case "adjective":
                        types.Adjectives = type.value;
                        break;
                    case "Adverb":
                        types.Adverb = type.value;
                        break;
                    case "Verb":
                        types.Verb = type.value;
                        break;
                }
            }
            
            return Ok(JsonConvert.SerializeObject(types));
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