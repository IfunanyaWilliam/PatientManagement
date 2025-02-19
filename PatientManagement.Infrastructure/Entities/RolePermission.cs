
namespace PatientManagement.Infrastructure.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class RolePermission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public IdentityRole<Guid> Role { get; set; }
        public Permission Permission { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
