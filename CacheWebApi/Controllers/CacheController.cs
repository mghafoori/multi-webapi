using System;
using Cache.WebApi.Attributes;
using CacheWebApi.Models;
using CacheWebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cache.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize]
    public class CacheController : ControllerBase
    {
        private readonly ILogger<CacheController> _logger;
        private readonly ICacheStore _cache;
        private readonly IValidator<CacheItemModel> _validator;

        public CacheController(
            ILogger<CacheController> logger,
            ICacheStore cache,
            IValidator<CacheItemModel> validator
        ) =>
            (_logger, _cache, _validator)
            =
            (logger, cache, validator);

        [HttpGet]
        [Route("{key}")]
        [CustomCulture("en", "fr", "es")]
        public IActionResult Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("'key' is required.");
            }
            var value = _cache.Get(key);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPost]
        [CustomCulture("en", "fr")]
        public ActionResult Add([FromBody] CacheItemModel model)
        {
            try
            {
                var (validationResult, validationMessage) = _validator.Validate(model);
                if (!validationResult)
                {
                    return BadRequest(validationMessage);
                }
                if (_cache.Add(model.Key, model.Value))
                {
                    return Ok();
                }
                return Conflict();
            }
            catch (Exception ex)
            {
                var logId = Guid.NewGuid();
                _logger.LogError(ex, ex.Message, logId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    logId
                );
            }
        }

        [HttpPut]
        public ActionResult Update([FromBody] CacheItemModel model)
        {
            try
            {
                var (validationResult, validationMessage) = _validator.Validate(model);
                if (!validationResult)
                {
                    return BadRequest(validationMessage);
                }
                if (_cache.Update(model.Key, model.Value))
                {
                    return Ok();
                }
                return Conflict();
            }
            catch (Exception ex)
            {
                var logId = Guid.NewGuid();
                _logger.LogError(ex, ex.Message, logId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    logId
                );
            }
        }

        [HttpDelete]
        [Route("{key}")]
        public ActionResult Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("'key' is required.");
            }
            if (_cache.Delete(key))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}