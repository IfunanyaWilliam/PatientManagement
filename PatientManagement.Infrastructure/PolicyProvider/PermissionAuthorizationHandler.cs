
namespace PatientManagement.Infrastructure.PolicyProvider
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Authorization;
    using Common.Enums;
    using Common.Utilities;

    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
           AuthorizationHandlerContext context, 
           PermissionRequirement requirement)
        {
            if (requirement.PermissionOperator == PermissionOperator.And)
            {
                foreach (var permission in requirement.Permissions)
                {
                    if (!context.User.HasClaim(PermissionRequirement.ClaimType, permission))
                    {
                        context.Fail();
                        throw new CustomException("Access denied: Insufficient permissions to perform this action", 
                            StatusCodes.Status403Forbidden);
                    }
                }

                // identity has all required permissions
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            foreach (var permission in requirement.Permissions)
            {
                if (context.User.HasClaim(PermissionRequirement.ClaimType, permission))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            // identity does not have any of the required permissions
            context.Fail();
            throw new CustomException("Access denied: Insufficient permission to perform this action", 
                StatusCodes.Status403Forbidden);
        }
    }
}
