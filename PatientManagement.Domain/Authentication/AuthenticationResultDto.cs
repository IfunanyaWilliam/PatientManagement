
namespace PatientManagement.Domain.Authentication
{
    public class AuthenticationResultDto
    {
        public AuthenticationResultDto(
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
