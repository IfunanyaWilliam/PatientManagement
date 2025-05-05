
namespace PatientManagement.Application.Queries.Authentication.Handler
{
    using Microsoft.AspNetCore.Http;
    using Parameters;
    using Results;
    using Utilities;
    using Application.Interfaces.Services;
    using Interfaces.Handlers;

    public class GetRefreshTokenQueryHandler : IQueryHandler<GetRefreshTokenQueryParameters, GetAuthTokenQueryResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public GetRefreshTokenQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<GetAuthTokenQueryResult> HandleAsync(
            GetRefreshTokenQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _authenticationService.RefreshTokenAsync(refreshToken: parameters.RefreshToken);

            if (result is null)
                return new GetAuthTokenQueryResult(null, null);
             
            return new GetAuthTokenQueryResult(
                accessToken: result.AccessToken,
                refreshToken: result.RefreshToken);
        }
    }
}
