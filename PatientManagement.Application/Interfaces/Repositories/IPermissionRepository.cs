
namespace PatientManagement.Application.Interfaces.Repositories
{
    using PatientManagement.Domain.Permission;

    public interface IPermissionRepository
    {
        Task<Permission> CreatePermissionAsync(string name);

        Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);

        Task<List<string>> GetDecryptedPermissionsForRoleAsync(Guid roleId);

        Task<List<Permission>> GetPermissionsByRoleIdsAsync(IEnumerable<Guid> roleIds);

        Task<List<Guid>> GetRoleIdsByNamesAsync(IEnumerable<string> roleNames);
    }
}
