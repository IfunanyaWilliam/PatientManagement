
namespace PatientManagement.Api.Parameters
{
    public class GetRefreshTokenParameters
    {
        public GetRefreshTokenParameters(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; }
    }
}
