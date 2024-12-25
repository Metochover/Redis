using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        IDistributedCache _distiributedCache;
        public HomeController(IDistributedCache distributedCache)
        {
                _distiributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> Set(string name , string surname)
        {
            await _distiributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30),
            });
            await _distiributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30),
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var name = await _distiributedCache.GetStringAsync("name");
            var surnameBinary = await _distiributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameBinary);

            return Ok(name+" "+surname);
        }
    }
}
