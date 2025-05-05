
namespace PatientManagement.Infrastructure.Entities
{
    using Domain.ApplicationUser;
    using Domain.Professional;

    public class Professional
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? Qualification { get; set; }
        public string? License { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public UserRole Role { get; set; }
        public ProfessionalStatus ProfessionalStatus { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime DateModified { get; set; }
    }
}
