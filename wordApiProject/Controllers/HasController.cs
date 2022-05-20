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

    }
    }

