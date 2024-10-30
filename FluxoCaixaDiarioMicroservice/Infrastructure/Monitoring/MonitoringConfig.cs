using Elastic.Apm.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace FluxoCaixaDiarioMicroservice.Infrastructure.Monitoring
{
    public static class MonitoringConfig
    {
        public static IApplicationBuilder UseElasticApmMonitoring(this IApplicationBuilder app, IConfiguration configuration)
        {
            // Use o middleware do Elastic APM
            app.UseElasticApm(configuration);
            return app;
        }
    }
}
