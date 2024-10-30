using StackExchange.Redis;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Redis
{
    public static class RedisConfig
    {
        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetSection("Redis:ConnectionString").Value;

            var redis = ConnectionMultiplexer.Connect(redisConnectionString);

            services.AddSingleton<IConnectionMultiplexer>(redis);
            services.AddScoped<RedisCacheService, RedisCacheService>();

            return services;
        }
    }
}
