
namespace PatientManagement.Api.Extensions
{
    using Application.Extensions;
    using Asp.Versioning;
    using FluentValidation;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using PatientManagement.Api.Filters;
    using Validators;

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

            //disable automatic model state validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddValidatorsFromAssemblyContaining<ApproveProfessionalStatusParametersValidator>();
            services.ConfigureAuthorization(configuration);
            services.AddSwaggerConfiguration();

            services.AddApplicationDependencies();
            services.AddInfrastructureDependencies(configuration);
        }
    }
}
