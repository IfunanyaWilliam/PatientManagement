
namespace PatientManagement.Domain.Account
{
    public class CreateUserResultDto
    {
        public CreateUserResultDto(
            Guid userId,
            string email,
            string userRole,
            DateTime created)
        {
            UserId = userId;
            Email = email;
            UserRole = userRole;
            DateCreated = created;
        }

        public Guid UserId { get; }
        public string Email { get; }
        public string UserRole { get; }
        public DateTime DateCreated { get; }
    }
}
