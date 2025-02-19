
namespace PatientManagement.Api.Extensions
{
    using Asp.Versioning;
    using Application.Extensions;
    using Common.Extensions;
    using Infrastructure.Extensions;


    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); 
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.ConfigureAuthorization(configuration);
            services.AddSwaggerConfiguration();

            services.AddApplicationDependencies();
            services.AddDependenciesFromCommon();
            services.AddInfrastructureDependencies();
        }
    }
}
