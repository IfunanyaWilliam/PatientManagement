
namespace PatientManagement.Application.Queries.Authentication.Handler
{
    using Microsoft.AspNetCore.Http;
    using Parameters;
    using Results;
    using Utilities;
    using Application.Interfaces.Services;
    using Application.Interfaces.Handlers;

    public class GetAuthTokenQueryHandler : IQueryHandler<GetAuthTokenQueryParameters, GetAuthTokenQueryResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public GetAuthTokenQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<GetAuthTokenQueryResult> HandleAsync(
            GetAuthTokenQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _authenticationService.GetAuthTokenAsync(
                email: parameters.Email,
                password: parameters.Password);

            if (result is null)
                return new GetAuthTokenQueryResult(null, null);

            return new GetAuthTokenQueryResult(
                accessToken: result.AccessToken,
                refreshToken: result.RefreshToken);
        }
    }
}
