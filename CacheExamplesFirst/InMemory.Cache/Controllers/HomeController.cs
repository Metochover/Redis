using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;

namespace InMemory.Cache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        readonly IMemoryCache _memoryCache;
        public HomeController(IMemoryCache memoryCache)
        {
               _memoryCache = memoryCache;
        }

        [HttpPost]
        public void SetName(string name)
        {
            _memoryCache.Set("name", name);  //cache'e "key-value" formatında veri set ediyoruz.
        }

        [HttpGet("Get")]
        public string GetName()
        {
            return _memoryCache.Get<string>("name"); //cache'de "key"'i "name" olan veriyi okumaya çalışıyoruz
        }

        [HttpGet("TryGetValue")]
        public string TryGetName()
        {
            if (_memoryCache.TryGetValue<string>("name", out string name))//cache'de "key"'i "name" olan veriyi okumaya çalışıyoruz burada boolean bir değer döne bize
            {
                return name.Substring(2);
            }
            else
                return null;
        }

        [HttpPost("setDate")]
        public void SetDate()
        {
            _memoryCache.Set("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("date"); 
        }
    }
}
