
namespace PatientManagement.Application.Commands.Account.Paremeters
{
    using Common.Contracts;
    using Common.Enums;

    public class CreateUserCommandParameters : ICommand
    {
        public CreateUserCommandParameters(
            string email,
            string password,
            UserRole userRole) 
        {
            Email = email;
            Password = password;
            UserRole = userRole;
        }


        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
    }
}
