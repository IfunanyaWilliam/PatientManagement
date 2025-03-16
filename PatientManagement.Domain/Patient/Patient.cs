
namespace PatientManagement.Domain.Patient
{
    public class Patient
    {
        public Patient(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            string? email,
            bool isActive,
            string userRole,
            DateTime dateCreated,
            DateTime? dateModified) 
        {
            Id = id;
            ApplicationUserId = applicationUserId;
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Age = age;
            Email = email;
            IsActive = isActive;
            UserRole = userRole;
            DateCreated = dateCreated;
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
        public string UserRole { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
