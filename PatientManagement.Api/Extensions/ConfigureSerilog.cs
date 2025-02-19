using Serilog;

namespace PatientManagement.Api.Extensions
{
    public static class ConfigureSerilog
    {
        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) =>
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
            );

            return builder;
        }
    }
}
