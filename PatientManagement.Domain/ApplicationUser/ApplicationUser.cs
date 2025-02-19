
namespace PatientManagement.Domain.ApplicationUser
{
    public class ApplicationUser
    {
        public ApplicationUser(
            Guid id,
            string? firstName,
            string? middleName,
            string? lastName,
            string? email,
            DateTime createdDate,
            DateTime dateModified,
            bool isDeleted,
            UserRole userRole)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Email = email;
            CreatedDate = createdDate;
            DateModified = dateModified;
            IsDeleted = isDeleted;
            Role = userRole;
        }


        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
        public UserRole Role { get; set; }
    }
}
