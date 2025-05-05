
namespace PatientManagement.Application.Commands.Account.Paremeters
{
    using Domain.ApplicationUser;
    using Interfaces.Commands;

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
