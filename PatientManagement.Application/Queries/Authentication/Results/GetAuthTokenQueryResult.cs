
namespace PatientManagement.Application.Queries.Authentication.Results
{
    using Interfaces.Queries;

    public class GetAuthTokenQueryResult : IQueryResult
    {
        public GetAuthTokenQueryResult(
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
