

namespace PatientManagement.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using DbContexts;
    using Entities;
    using Repositories.Interfaces;
    using Repositories.Implementations;
    using Services.Implementation;
    using Services.Interfaces;
    using PolicyProvider;


    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "PatientManagementDb"));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Overrides the DefaultAuthorizationPolicyProvider
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            return services;
        }
    }
}
