using System.Text.Json;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Redis
{
  public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration)
    {
        var jsonValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, jsonValue, expiration);
    }

    public async Task<T> GetCacheValueAsync<T>(string key)
    {
        var jsonValue = await _database.StringGetAsync(key);
        if (jsonValue.IsNullOrEmpty)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonValue);
    }

    public async Task RemoveCacheValueAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
 }
}