
namespace PatientManagement.Application.Queries.Professional.Resulsts
{
    using Interfaces.Queries;

    public class GetProfessionalByIdQueryResult : IQueryResult
    {
        public GetProfessionalByIdQueryResult(
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
        public string UserRole { get; }
        public string ProfessionalStatus { get; }
        public DateTime DateCreated { get; }
        public DateTime? DateModified { get; }
    }
}
