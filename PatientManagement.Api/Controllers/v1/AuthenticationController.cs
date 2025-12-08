
namespace PatientManagement.Api.Controllers.v1
{
    using Application.Interfaces.Queries;
    using Application.Queries.Authentication.Parameters;
    using Application.Queries.Authentication.Results;
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PatientManagement.Api.Parameters;
    using PatientManagement.Api.Results;
    using PatientManagement.Application.Commands.Account.Paremeters;
    using PatientManagement.Application.Commands.Account.Result;
    using PatientManagement.Application.Interfaces.Commands;

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;

        public AuthenticationController(IQueryExecutor queryExecutor, ICommandExecutorWithResult commandExecutorWithResult)
        {
            _queryExecutor = queryExecutor;
            _commandExecutorWithResult = commandExecutorWithResult;
        }


        /// <summary>
        ///     POST: /api/v1/Authentication
        /// </summary>
        /// <remarks>
        ///     Add a patient.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500" >
        ///     Internal Server Error.
        /// </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAuthTokenResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthTokenAsync(
            GetAuthTokenParameter parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Invalid parameters");

            var result = await _queryExecutor
                .ExecuteAsync<GetAuthTokenQueryParameters, GetAuthTokenQueryResult>(
                    parameters: new GetAuthTokenQueryParameters(
                        email: parameters.Email,
                        password: parameters.Password),
                    ct: ct);

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


        /// <summary>
        ///     POST: /api/v1/Authentication
        /// </summary>
        /// <remarks>
        ///     Add a patient.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500" >
        ///     Internal Server Error.
        /// </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAuthTokenResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRefreshTokenAsync(
            GetRefreshTokenParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Invalid parameters");

            var result = await _queryExecutor
                .ExecuteAsync<GetRefreshTokenQueryParameters, GetAuthTokenQueryResult>(
                    parameters: new GetRefreshTokenQueryParameters(
                        refreshToken: parameters.RefreshToken),
                    ct: ct);

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


        /// <summary>
        ///     POST: /api/v1/Authentication
        /// </summary>
        /// <remarks>
        ///     Add a patient.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500" >
        ///     Internal Server Error.
        /// </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginWithFacebookAsync(
            LoginWithFacebookParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Invalid parameters");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<LoginWithFacebookCommandParameters, LoginWithFacebookCommandResult>(
                    command: new LoginWithFacebookCommandParameters(
                        accessToken: parameters.AccessToken),
                    ct: ct);

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
