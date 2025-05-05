
namespace PatientManagement.Api.Results
{
    public class GetPatientResult
    {
        public GetPatientResult(
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
        public string UserRole { get; }
        public DateTime DateCreated { get; }
        public DateTime? DateModified { get; }
    }
}
