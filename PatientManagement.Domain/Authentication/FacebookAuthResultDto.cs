
namespace PatientManagement.Domain.Authentication
{
    public class FacebookAuthResultDto
    {
        public FacebookAuthResultDto(
            Guid userId,
             string? accessToken,
            string? refreshToken)
        {
            UserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public Guid UserId { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
