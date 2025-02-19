
namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Application.Queries.Authentication.Parameters;
    using Application.Queries.Authentication.Results;
    using Common.Contracts;
    using Common.Parameters;
    using Common.Results;

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        private readonly IQueryExecutor _queryExecutor;

        public AuthenticationController(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAuthTokenResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthTokenAsync(
            GetAuthTokenParameter parameters,
            CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                return BadRequest("Invalid parameters");

            var result = await _queryExecutor
                .ExecuteAsync<GetAuthTokenQueryParameters, GetAuthTokenQueryResult>(
                    parameters: new GetAuthTokenQueryParameters(
                        email: parameters.Email,
                        password: parameters.Password),
                    ct: cancellationToken);

            if (result == null
                || result.AccessToken == null
                || string.IsNullOrWhiteSpace(result.AccessToken))
            {
                ModelState.AddModelError("login.Unauthorized", "Access not authorized.");
                return BadRequest(ModelState);
            }

            return Ok(new AuthenticationResult(
                accessToken: result.AccessToken,
                refreshToken: result.RefreshToken
            ));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAuthTokenResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRefreshTokenAsync(
            GetRefreshTokenParameters parameters,
            CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                return BadRequest("Invalid parameters");

            var result = await _queryExecutor
                .ExecuteAsync<GetRefreshTokenQueryParameters, GetAuthTokenQueryResult>(
                    parameters: new GetRefreshTokenQueryParameters(
                        refreshToken: parameters.RefreshToken),
                    ct: cancellationToken);

            if (result == null
                || result.AccessToken == null
                || string.IsNullOrWhiteSpace(result.AccessToken))
            {
                ModelState.AddModelError("login.Unauthorized", "Access not authorized.");
                return BadRequest(ModelState);
            }

            return Ok(new AuthenticationResult(
                accessToken: result.AccessToken,
                refreshToken: result.RefreshToken
            ));
        }
    }
}
