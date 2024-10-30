using Serilog;
using Serilog.Sinks.Elasticsearch;


namespace FluxoCaixaDiarioMicroservice.Infrastructure.Logging
{
    public static class LoggingConfig
    {
        public static void ConfigureLogging(IConfiguration configuration)
        {
            var elasticUri = configuration["ElasticConfiguration:Uri"];

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"fluxo-caixa-diario-logs-{DateTime.UtcNow:yyyy-MM}"
                })
                .CreateLogger();
        }

        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);
            return services;
        }
    }
}
