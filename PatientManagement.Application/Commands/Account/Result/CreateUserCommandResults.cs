
namespace PatientManagement.Application.Commands.Account.Result
{
    using Common.Contracts;


    public class CreateUserCommandResults : ICommandResult
    {
        public CreateUserCommandResults(
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
