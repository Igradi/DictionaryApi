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
            for(int i = 0; i < 4; i++)
            {
                allWords[i] = new AllWordsModel();
            }
            int countNouns = (from Words in _context.Words where Words.WordType == "noun" select Words).Count();
            int countAdjectives = (from Words in _context.Words where Words.WordType == "adjective" select Words).Count();
            int countAdverb = (from Words in _context.Words where Words.WordType == "adverb" select Words).Count();
            int countVerb = (from Words in _context.Words where Words.WordType == "verb" select Words).Count();

            allWords[0].name = "noun";
            allWords[0].value = countNouns;
            allWords[1].name= "adjective";
            allWords[1].value = countAdjectives;
            allWords[2].name = "adverb";
            allWords[2].value = countAdverb;
            allWords[3].name = "verb";
            allWords[3].value = countVerb;
            string jsonString = JsonConvert.SerializeObject(allWords);
            return Ok(jsonString);
        }


        [HttpGet("{id},{wordName},{wordType}")]
        public async Task<ActionResult> PutNewWord(int id,string wordName,string wordType)
        {
            
            var newWord = new Words();
            var newHas = new Has();
            
            newWord.WordName = wordName;
            newWord.WordType = wordType;
            var wordExists = from Words in _context.Words where (Words.WordName == wordName) select Words.Id;
            if (wordExists.Count() > 0)
            {
                var link = from Has in _context.Hass where (Has.WordId == wordExists.First()) select Has;
                var listOfIds = (from Has in _context.Hass where (Has.WordId == wordExists.First()) select Has.UserId).ToList();
              
                if (!listOfIds.Contains(id)) {  
               newHas.WordId = link.First().WordId;
               newHas.UserId = id;
              
              _context.Hass.Add(newHas);
                   await _context.SaveChangesAsync();
                    return Ok();

                }
                return Ok();

            }
            else {
                _context.Words.Add(newWord);
                await _context.SaveChangesAsync();

                var IdQuery = from Words in _context.Words where (Words.WordName == newWord.WordName) select Words.Id;

                newHas.WordId = IdQuery.First();
                newHas.UserId = id;

                _context.Hass.Add(newHas);
                await _context.SaveChangesAsync();

                var IdHasQuery = from Hass in _context.Hass where (Hass.WordId == newHas.WordId) select Hass.Id;
                var dbWord = await _context.Words.FindAsync(newHas.WordId);
                dbWord.HasId = IdHasQuery.First();
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
        
    
        [HttpGet]
        public async Task<ActionResult<List<Words>>> Get()
        {
            
            return Ok(await _context.Words.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<int>>GetNumOfWords(int id)
        {
            int counter = 0;
            counter =  _context.Hass.Count(Hass=>Hass.UserId == id);
            
            return( counter);
        }

        [Route(("/api/[controller]/getTypes"))]
        [HttpGet]
        public async Task<ActionResult<string>> GetNumOfTypes(int id)
        {
            TypesModel types = new TypesModel();
            List<int> wordIds = new List<int>();
            wordIds = (from Hass in _context.Hass where Hass.UserId == id select Hass.WordId).ToList();
           
            foreach(var wordId in wordIds)
            {
                
                var word = await _context.Words.FindAsync(wordId);
               
                switch (word.WordType)
                {
                    case "noun":
                        types.nouns++;
                        break;
                    case "verb":
                        types.verb++;
                        break;
                    case "adjective":
                        types.adjectives++;
                        break;
                    case "adverb":
                        types.adverb++;
                        break;
                }

            }
            string jsonString = JsonConvert.SerializeObject(types);
            Console.WriteLine(jsonString);
            return Ok(jsonString) ;
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