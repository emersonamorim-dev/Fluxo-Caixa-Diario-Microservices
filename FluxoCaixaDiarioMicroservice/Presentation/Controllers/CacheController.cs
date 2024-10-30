using FluxoCaixaDiarioMicroservice.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly RedisCacheService _redisCacheService;

        public CacheController(RedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetCache([FromQuery] string key, [FromBody] object value)
        {
            await _redisCacheService.SetCacheValueAsync(key, value, TimeSpan.FromMinutes(30));
            return Ok("Valor definido no cache com sucesso.");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCache([FromQuery] string key)
        {
            var value = await _redisCacheService.GetCacheValueAsync<object>(key);
            if (value == null)
            {
                return NotFound("Chave não encontrada no cache.");
            }
            return Ok(value);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveCache([FromQuery] string key)
        {
            await _redisCacheService.RemoveCacheValueAsync(key);
            return Ok("Valor removido do cache com sucesso.");
        }
    }
}
