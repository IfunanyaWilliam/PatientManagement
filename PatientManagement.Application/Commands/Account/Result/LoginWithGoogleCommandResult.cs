
namespace PatientManagement.Application.Commands.Account.Result
{
    using Interfaces.Commands;

    public class LoginWithGoogleCommandResult : ICommandResult
    {
        public LoginWithGoogleCommandResult(
            string accessToken, 
            string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
