
namespace PatientManagement.Infrastructure.Entities
{
    using Microsoft.AspNetCore.Identity;
    using Domain.ApplicationUser;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; } = false;
        public UserRole Role { get; set; }
        public List<ExternalLogin> ExternalLogins { get; set; } = new();
    }
}
