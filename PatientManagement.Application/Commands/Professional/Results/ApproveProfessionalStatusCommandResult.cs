
namespace PatientManagement.Application.Commands.Professional.Results
{
    using Common.Contracts;

    public class ApproveProfessionalStatusCommandResult : ICommandResult
    {
        public ApproveProfessionalStatusCommandResult(
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
            string email,
            bool isActive,
            string userRole,
            string professionalStatus,
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
            Qualification = qualification;
            License = license;
            Email = email;
            IsActive = isActive;
            UserRole = userRole;
            ProfessionalStatus = professionalStatus;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }


        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; }
        public string? MiddleName { get; }
        public string? LastName { get; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public string? Qualification { get; set; }
        public string? License { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public string UserRole { get; set; }
        public string ProfessionalStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
