
namespace PatientManagement.Common.Results
{
    using Enums;

    public class CreatePatientResult
    {

        public CreatePatientResult(
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
            UserRole userRole,
            DateTime createdDate)
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
            CreatedDate = createdDate;
        }

        public Guid Id { get; }
        public Guid ApplicationUserId { get; }
        public string? Title { get; }
        public string? FirstName { get; }
        public string? MiddleName { get; }
        public string? LastName { get; }
        public string? PhoneNumber { get; }
        public int Age { get; }
        public string? Email { get; }
        public bool IsActive { get; }
        public UserRole UserRole { get; }
        public DateTime CreatedDate { get; }
    }
}
