

namespace PatientManagement.Application.Commands.Account.Paremeters
{
    using Interfaces.Commands;

    public class LoginWithGoogleCommandParameters : ICommand
    {
        public LoginWithGoogleCommandParameters(string idToken)
        {
            IdToken = idToken;
        }

        public string IdToken { get; set; }
    }
}
