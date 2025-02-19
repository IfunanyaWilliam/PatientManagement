
namespace PatientManagement.Infrastructure.Entities
{
    using Microsoft.AspNetCore.Identity;
    using Common.Enums;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime DateModified { get; set; } = DateTime.UtcNow.AddHours(1);
        public bool IsDeleted { get; set; } 
        public UserRole Role { get; set; }
    }
}
