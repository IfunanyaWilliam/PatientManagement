
namespace PatientManagement.Infrastructure.PolicyProvider
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using DbContexts;
    using Services.Interfaces;


    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionPolicyProvider(
            IOptions<AuthorizationOptions> options, 
            IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Check if the policy name matches your expected format
            if (policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                // Extract permissions from the policy name
                var permissions = PermissionAuthorizeAttribute.GetPermissionsFromPolicy(policyName);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var _encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();    

                    // Check if all the permissions exist in the database
                    var permissionExists = await dbContext.Permissions
                        .Where(p => permissions.Contains(_encryptionService.Decrypt(p.EncryptedName)))
                        .ToListAsync();

                    if (permissionExists.Count == permissions.Length)
                    {
                        var requirement = new PermissionRequirement(PermissionAuthorizeAttribute.GetOperatorFromPolicy(policyName), permissions);
                        return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
                    }
                }
            }

            // Fallback to default behavior if no permission found
            return await base.GetPolicyAsync(policyName);
        }
    }
}
