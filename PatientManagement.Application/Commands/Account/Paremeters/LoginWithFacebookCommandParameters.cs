

namespace PatientManagement.Application.Commands.Account.Paremeters
{
    using Interfaces.Commands;


    public class LoginWithFacebookCommandParameters : ICommand
    {
        public LoginWithFacebookCommandParameters(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
    }
}
