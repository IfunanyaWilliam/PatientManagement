
namespace PatientManagement.Api.Extensions
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.OpenApi.Models;

    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PatientManagement", Version = "v1" });

                //c.SwaggerDoc("v2", new OpenApiInfo { Title = "PatientManagement", Version = "v2" });

                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the input below. \r\n\r\n Example :'Bearer 124fsfs' "
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[]{ }
                    }
                });

                //c.OperationFilter<RemoveVersionParameterFilter>();
                //c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            });
        }
    }
}
