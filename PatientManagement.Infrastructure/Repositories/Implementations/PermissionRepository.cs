
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using DbContexts;
    using Services.Interfaces;
    using PatientManagement.Domain.Permission;
    using Application.Interfaces.Repositories;

    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;

        public PermissionRepository(
            AppDbContext dbContext, 
            IEncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
        }
        
        public async Task<Permission> CreatePermissionAsync(string name)
        {
            var permission = new Entities.Permission
            {
                EncryptedName = _encryptionService.Encrypt(name)
            };

            await _dbContext.Permissions.AddAsync(permission);
            await _dbContext.SaveChangesAsync();

            return new Permission(
                id: permission.Id,
                encryptedName: permission.EncryptedName,
                dateCreated: permission.DateCreated,
                dateModified: permission.DateModified);
        }

        public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var rolePermission = new Entities.RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await _dbContext.RolePermissions.AddAsync(rolePermission);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> GetDecryptedPermissionsForRoleAsync(Guid roleId)
        {
            var permissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission.EncryptedName)
                .ToListAsync();

            return permissions.Select(p => _encryptionService.Decrypt(p)).ToList();
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdsAsync(IEnumerable<Guid> roleIds)
        {
            var permissions = await _dbContext.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync();

            return permissions.Select(p =>
                    new Permission(
                        id: p.Id,
                        encryptedName: p.EncryptedName,
                        dateCreated: p.DateCreated,
                        dateModified: p.DateModified)).ToList();
        }

        public async Task<List<Guid>> GetRoleIdsByNamesAsync(IEnumerable<string> roleNames)
        {
            return await _dbContext.Roles
                .Where(r => roleNames.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();
        }
    }
}
