

namespace PatientManagement.Common.Parameters
{
    using Common.Enums;

    public class CreateUserParameters
    {
        public CreateUserParameters()
        {
        }

        public CreateUserParameters(
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
