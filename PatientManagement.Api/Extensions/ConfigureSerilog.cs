using Serilog;

namespace PatientManagement.Api.Extensions
{
    public static class ConfigureSerilog
    {
        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder applicationBuilder)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(applicationBuilder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            applicationBuilder.Logging.ClearProviders();
            applicationBuilder.Logging.AddSerilog(logger);
            return applicationBuilder;
        }
    }
}
