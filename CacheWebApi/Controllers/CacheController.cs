using Microsoft.AspNetCore.Mvc;

namespace Cache.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheStore _cache;

        public CacheController(ICacheStore cache) => _cache = cache;

        [HttpGet("key")]
        public IActionResult Get(string key)
        {
            var value = _cache.Get(key);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPost]
        public ActionResult Add(string key, string value)
        {
            if (_cache.Add(key, value))
            {
                return Ok();
            }
            return Conflict();
        }

        [HttpPut]
        public ActionResult Update(string key, string value)
        {
            if (_cache.Update(key, value))
            {
                return Ok();
            }
            return Conflict();
        }

        [HttpDelete]
        public ActionResult Delete(string key)
        {
            if (_cache.Delete(key))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}