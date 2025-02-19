namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Application.Commands.Account.Paremeters;
    using Application.Commands.Account.Result;
    using Common.Contracts;
    using Common.Parameters;
    using Common.Results;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;

        public AccountController(
            ICommandExecutorWithResult commandExecutorWithResult)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser(
            CreateUserParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<CreateUserCommandParameters, CreateUserCommandResults>(
                    command: new CreateUserCommandParameters(
                        email: parameters.Email,
                        password: parameters.Password,
                        userRole: parameters.UserRole),
                    ct: ct);

            return Ok(new CreateUserResult(
                userId: result.UserId,
                email: result.Email,
                userRole: result.UserRole,
                created: result.DateCreated));
        }
    }
}
