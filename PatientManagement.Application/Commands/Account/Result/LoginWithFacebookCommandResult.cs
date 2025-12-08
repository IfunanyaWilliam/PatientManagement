
namespace PatientManagement.Application.Commands.Account.Result
{
    using Interfaces.Commands;


    public class LoginWithFacebookCommandResult : ICommandResult
    {
        public LoginWithFacebookCommandResult(
            string? accessToken,
            string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
