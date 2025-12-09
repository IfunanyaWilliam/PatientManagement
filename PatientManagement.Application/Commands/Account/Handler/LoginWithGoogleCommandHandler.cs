

namespace PatientManagement.Application.Commands.Account.Handler
{
    using Interfaces.Handlers;
    using Microsoft.AspNetCore.Http;
    using Paremeters;
    using PatientManagement.Application.Interfaces.Repositories;
    using PatientManagement.Application.Interfaces.Services;
    using PatientManagement.Application.Utilities;
    using Result;


    public class LoginWithGoogleCommandHandler :
        ICommandHandlerWithResult<LoginWithGoogleCommandParameters, LoginWithGoogleCommandResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IGoogleAuthService _googleAuthService;

        public LoginWithGoogleCommandHandler(
            IAccountRepository accountRepository, 
            IAuthenticationService authenticationService,
            IGoogleAuthService googleAuthService) 
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _googleAuthService = googleAuthService ?? throw new ArgumentNullException(nameof(googleAuthService));
        }

        public async Task<LoginWithGoogleCommandResult> HandleAsync(
            LoginWithGoogleCommandParameters command, 
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(command.IdToken))
                throw new ArgumentException("Invalid parameters");

            var googleValidationResult = await _googleAuthService.ValidateGoogleTokenAsync(command.IdToken);

            if (googleValidationResult != null || string.IsNullOrWhiteSpace(googleValidationResult.Email))
            {
                throw new UnauthorizedAccessException("Invalid Google token");
            }

            var internalJwtToken = await _authenticationService.GetAuthTokenAsync(googleValidationResult.Email);

            if (internalJwtToken == null)
            {
                throw new CustomException("Token not generated", StatusCodes.Status500InternalServerError);
            }

            var isTokenInserted = await _accountRepository.InsertExternalLoginAsync(
                userId: internalJwtToken.UserId,
                provider: "Google",
                providerUserId: googleValidationResult.Sub,
                cancellationToken: ct);

            if (!isTokenInserted)
            {
                throw new CustomException("Token not generated", StatusCodes.Status500InternalServerError);
            }

            return new LoginWithGoogleCommandResult(
                accessToken: internalJwtToken.AccessToken,
                refreshToken: internalJwtToken.RefreshToken);
        }
    }
}
