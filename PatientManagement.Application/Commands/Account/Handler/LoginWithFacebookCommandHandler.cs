
namespace PatientManagement.Application.Commands.Account.Handler
{
    using Microsoft.AspNetCore.Http;
    using Interfaces.Repositories;
    using Interfaces.Handlers;
    using Interfaces.Services;
    using Paremeters;
    using Utilities;
    using Result;
    

    public class LoginWithFacebookCommandHandler :
        ICommandHandlerWithResult<LoginWithFacebookCommandParameters, LoginWithFacebookCommandResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IAuthenticationService _authenticationService;

        public LoginWithFacebookCommandHandler(
            IAccountRepository accountRepository, 
            IFacebookAuthService facebookAuthService,
            IAuthenticationService authenticationService)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _facebookAuthService = facebookAuthService ?? throw new ArgumentNullException(nameof(facebookAuthService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }


        public async Task<LoginWithFacebookCommandResult> HandleAsync(
            LoginWithFacebookCommandParameters command, 
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(command.AccessToken))
                throw new ArgumentException("Invalid parameters");

            var facebookValidationResult = await _facebookAuthService.VerifyFacebookTokenAsync(command.AccessToken);

            if(facebookValidationResult == null || string.IsNullOrWhiteSpace(facebookValidationResult.Email))
            {
                throw new UnauthorizedAccessException("Invalid Facebook token");
            }

            var tokenResult = await _authenticationService.GetAuthTokenAsync(facebookValidationResult.Email);

            if (tokenResult == null)
            {
                throw new CustomException("Token not generated", StatusCodes.Status500InternalServerError);
            }

            var isTokenInserted = await _accountRepository.InsertFacebookLoginAsync(
                userId: tokenResult.UserId,
                facebookId: facebookValidationResult.Id,
                cancellationToken: ct);

            if (!isTokenInserted)
            {
                throw new CustomException("Token not generated", StatusCodes.Status500InternalServerError);
            }

            return new LoginWithFacebookCommandResult(
                accessToken: tokenResult.AccessToken,
                refreshToken: tokenResult.RefreshToken);
        }
    }
}
