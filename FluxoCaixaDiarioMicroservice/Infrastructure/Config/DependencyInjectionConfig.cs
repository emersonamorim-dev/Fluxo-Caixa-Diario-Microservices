using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Infrastructure.Db;
using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using FluxoCaixaDiarioMicroservice.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Config
{
    public static class DependencyInjectionConfig
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configura MongoDB
            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = configuration["MongoSettings:DatabaseName"];
                return client.GetDatabase(databaseName);
            });

            services.AddScoped<ILancamentoRepository, LancamentoRepository>();
            services.AddScoped<IConsolidadoDiarioRepository, ConsolidadoDiarioRepository>();
        }
    }
}