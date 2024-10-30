using System;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Redis
{
    public static class RedisRetryHelper
    {
        public static ConnectionMultiplexer ConnectWithRetry(ConfigurationOptions configurationOptions, int retryCount = 5)
        {
            int currentRetry = 0;
            while (true)
            {
                try
                {
                    return ConnectionMultiplexer.Connect(configurationOptions);
                }
                catch (RedisConnectionException)
                {
                    currentRetry++;
                    if (currentRetry >= retryCount)
                    {
                        throw; 
                    }
                    Console.WriteLine($"Tentando reconectar ao Redis ({currentRetry}/{retryCount})...");
                    System.Threading.Thread.Sleep(2000); // Aguardar 2 segundos entre as tentativas
                }
            }
        }
    }
}