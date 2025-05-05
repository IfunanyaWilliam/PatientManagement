
namespace PatientManagement.Api.Results
{
    public class GetAuthTokenResult
    {
        public GetAuthTokenResult(
            string token,
            int expiresIn,
            string refreshToken)
        {
            Token = token;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// JWT Token for the Authentication System
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Time of expiration in seconds
        /// </summary>
        public int ExpiresIn { get; }

        /// <summary>
        /// Refresh Token for retrieving new Access Token
        /// </summary>
        public string RefreshToken { get; }
    }
}
