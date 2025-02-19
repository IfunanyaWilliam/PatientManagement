
using PatientManagement.Domain.ApplicationUser;

namespace PatientManagement.Domain.Patient
{
    public class Patient
    {
        public Patient(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? phoneNumber,
            int age,
            bool isActive,
            bool isDeleted,
            UserRole role,
            DateTime createdDate,
            DateTime dateModified) 
        {
            Id = id;
            ApplicationUserId = applicationUserId;
            Title = title;
            PhoneNumber = phoneNumber;
            Age = age;
            IsActive = isActive;
            IsDeleted = isDeleted;
            Role = role;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }


        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
    }
}
