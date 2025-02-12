using PatientManagement.Domain.ApplicationUser;

namespace PatientManagement.Infrastructure.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; } 
        public UserRole Role { get; set; }
    }
}
