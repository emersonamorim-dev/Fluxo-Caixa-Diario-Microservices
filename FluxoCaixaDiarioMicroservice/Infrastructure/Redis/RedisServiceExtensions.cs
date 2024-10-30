using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Redis
{
    public static class RedisServiceExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisHost");

            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentException("Redis connection string not provided in configuration.");
            }

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configurationOptions = ConfigurationOptions.Parse(redisConnectionString, true);
                configurationOptions.AbortOnConnectFail = false; // Permite continuar tentando conectar ao Redis
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }
    }
}