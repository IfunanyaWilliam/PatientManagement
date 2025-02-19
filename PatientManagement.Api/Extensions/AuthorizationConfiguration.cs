
namespace PatientManagement.Api.Extensions
{
    using System.Text;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Authentication.JwtBearer;


    public static class AuthorizationConfiguration
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //var audience = configuration["JwtSettings:Audience"];

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    //ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();

                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            logger.LogError("Token has expired");
                        }
                        else if (context.Exception is SecurityTokenInvalidSignatureException)
                        {
                            logger.LogError("Token has invalid signature");
                        }
                        else if (context.Exception is SecurityTokenInvalidIssuerException)
                        {
                            logger.LogError("Token has invalid issuer");
                        }
                        else if (context.Exception is SecurityTokenInvalidAudienceException)
                        {
                            logger.LogError("Token has invalid audience");
                        }

                        logger.LogError(
                            "Authentication failed. Exception Type: {Type}, Error: {Error}",
                            context.Exception.GetType().Name,
                            context.Exception.Message);

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();

                        var token = context.SecurityToken as JwtSecurityToken;
                        logger.LogInformation(
                            "Token validated. Subject: {Subject}, Issuer: {Issuer}",
                            token?.Subject ?? "Unknown",
                            token?.Issuer?.FirstOrDefault());

                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogInformation(
                            "Token received in {Scheme} scheme. Token: {Token}",
                            context.Scheme.Name ?? "Unknown",
                            context.Token?.Substring(0, Math.Min(50, context.Token?.Length ?? 0)));
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogWarning(
                            "Authentication challenge issued. Error: {Error}, ErrorDescription: {ErrorDescription}",
                            context?.Error,
                            context?.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                // require all users to be authenticated by default
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}
