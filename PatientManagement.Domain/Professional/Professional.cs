using PatientManagement.Domain.ApplicationUser;

namespace PatientManagement.Domain.Professional
{
    public class Professional
    {
        public Professional(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? phoneNumber,
            int age,
            string? qualification,
            string? license,
            bool isActive,
            bool isDeleted,
            UserRole role,
            ProfessionalStatus professionalStatus,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            ApplicationUserId = applicationUserId;
            Title = title;
            PhoneNumber = phoneNumber;
            Age = age;
            Qualification = qualification;
            License = license;
            IsActive = isActive;
            IsDeleted = isDeleted;
            Role = role;
            ProfessionalStatus = professionalStatus;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? Qualification { get; set; }
        public string? License { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public UserRole Role { get; set; }
        public ProfessionalStatus ProfessionalStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
    }
}
