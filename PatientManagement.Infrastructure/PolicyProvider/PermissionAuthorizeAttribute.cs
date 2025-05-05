
namespace PatientManagement.Infrastructure.PolicyProvider
{
    using Microsoft.AspNetCore.Authorization;


    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string PolicyPrefix = "PERMISSION_";
        private const string Separator = "_";

        /// <summary>
        /// Initializes the attribute with multiple permissions
        /// </summary>
        /// <param name="permissionOperator">The operator to use when verifying the permissions provided</param>
        /// <param name="permissions">The list of permissions</param>
        public PermissionAuthorizeAttribute(PermissionOperator permissionOperator, params string[] permissions)
        {
            // E.g: PERMISSION_1_ViewTransactions_ViewTransactionDetails..
            Policy = $"{PolicyPrefix}{(int)permissionOperator}{Separator}{string.Join(Separator, permissions)}";
        }

        /// <summary>
        /// Initializes the attribute with a single permission
        /// </summary>
        /// <param name="permission">The permission</param>
        public PermissionAuthorizeAttribute(string permission)
        {
            // E.g: PERMISSION_1_ViewTransactions..
            Policy = $"{PolicyPrefix}{(int)PermissionOperator.And}{Separator}{permission}";
        }

        public static PermissionOperator GetOperatorFromPolicy(string policyName)
        {
            var @operator = int.Parse(policyName.AsSpan(PolicyPrefix.Length, 1));
            return (PermissionOperator)@operator;
        }

        public static string[] GetPermissionsFromPolicy(string policyName)
        {
            return policyName.Substring(PolicyPrefix.Length + 2)
                .Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
