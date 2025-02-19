
namespace PatientManagement.Common.Results
{
    using Enums;

    public class ApproveProfessionalStatusResult
    {
        public ApproveProfessionalStatusResult(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            string? qualification,
            string? license,
            string? email,
            bool isActive,
            UserRole userRole,
            ProfessionalStatus professionalStatus,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            ApplicationUserId = applicationUserId;
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Age = age;
            Qualification = qualification;
            License = license;
            Email = email;
            IsActive = isActive;
            UserRole = userRole;
            ProfessionalStatus = professionalStatus;
            CreatedDate = createdDate;
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
        public string? Qualification { get; }
        public string? License { get; }
        public string? Email { get; }
        public bool IsActive { get; }
        public UserRole UserRole { get; }
        public ProfessionalStatus ProfessionalStatus { get; }
        public DateTime CreatedDate { get; }
        public DateTime DateModified { get; }
    }
}
