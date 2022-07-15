using Microsoft.AspNetCore.Mvc;
using wordApiProject.Models;
using Newtonsoft.Json;
namespace wordApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWordsController : ControllerBase
    {
        private readonly DataContext _context;

        public UserWordsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{USERID}")]
        public async IAsyncEnumerable<List<Words>> GetWords(int USERID)
        {
            var HasIds = (from UserWord in _context.UserWords where UserWord.UserId == USERID select UserWord.Id).ToList();
            var targetWords = (from Words in _context.Words where HasIds.Contains(Words.HasId) select Words).ToListAsync();
            yield return await targetWords;
        }
        [Route(("/api/[controller]/topWords"))]
        [HttpGet]
        public async Task<ActionResult> TopWords()
        {
            List<RepetitionModel> topWords = new List<RepetitionModel>();

            var ordered = (from UserWord in _context.UserWords select UserWord.WordId).GroupBy(x => x).Where(g => g.Count() > 1).Select(y => new { Element = y.Key, Counter = y.Count() }).Take(10).OrderByDescending(x => x.Counter).ToList();

            foreach (var word in ordered)
            {
                RepetitionModel newEntry = new RepetitionModel();
                newEntry.WordName = _context.Words.Find(word.Element).WordName;
                newEntry.Repetitions = word.Counter;
                topWords.Add(newEntry);
            }
            string jsonString = JsonConvert.SerializeObject(topWords);
            return Ok(jsonString);
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
            var wordsId = (from UserWord in _context.UserWords where UserWord.UserId == id select UserWord.WordId).ToList();
            var userWordIds = from UserWord in _context.UserWords where UserWord.UserId == id select UserWord;
            foreach (UserWord userWord in userWordIds)
            {
                _context.UserWords.Remove(userWord);
            }
            _context.SaveChanges();
            return Ok(id);
        }
    }
}

