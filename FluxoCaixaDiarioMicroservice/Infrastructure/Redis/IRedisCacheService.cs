namespace FluxoCaixaDiarioMicroservice.Infrastructure.Redis
{
    public interface IRedisCacheService
    {
        Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration);
        Task<T> GetCacheValueAsync<T>(string key);
        Task RemoveCacheValueAsync(string key);
    }
}
