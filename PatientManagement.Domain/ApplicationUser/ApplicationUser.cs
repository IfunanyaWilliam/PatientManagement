
namespace PatientManagement.Domain.ApplicationUser
{
    public class ApplicationUser
    {
        public ApplicationUser(
            Guid id,
            string? email,
            DateTime createdDate,
            DateTime dateModified,
            bool isDeleted,
            UserRole userRole)
        {
            Id = id;
            Email = email;
            DateCreated = createdDate;
            DateModified = dateModified;
            IsDeleted = isDeleted;
            Role = userRole;
        }


        public Guid Id { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
        public UserRole Role { get; set; }
    }
}
