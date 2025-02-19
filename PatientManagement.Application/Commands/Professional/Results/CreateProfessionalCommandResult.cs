
namespace PatientManagement.Application.Commands.Professional.Results
{
    using Common.Contracts;
    using Common.Enums;

    public class CreateProfessionalCommandResult : ICommandResult
    {
        public CreateProfessionalCommandResult(
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
            Qualification = qualification;
            License = license;
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
        public string? Qualification { get; }
        public string? License { get; }
        public string? Email { get; }
        public bool IsActive { get; }
        public UserRole UserRole { get; }
        public DateTime CreatedDate { get; }
    }
}
