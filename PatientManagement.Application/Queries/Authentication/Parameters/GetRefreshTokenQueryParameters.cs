
namespace PatientManagement.Application.Queries.Authentication.Parameters
{
    using Interfaces.Queries;

    public class GetRefreshTokenQueryParameters : IQueryParameters
    {
        public GetRefreshTokenQueryParameters(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; }
    }
}
