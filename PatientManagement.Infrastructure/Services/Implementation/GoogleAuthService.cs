

namespace PatientManagement.Infrastructure.Services.Implementation
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Application.Utilities;
    using Domain.Authentication;
    using Application.Interfaces.Services;


    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly ILogger<GoogleAuthService> _logger;
        private readonly INetworkService _networkService;
        private readonly IConfiguration _configuration;
        private readonly string _ClientId;


        public GoogleAuthService(IConfiguration configuration, INetworkService networkService, ILogger<GoogleAuthService> logger) 
        {
            _configuration = configuration;
            _networkService = networkService;
            _logger = logger;
            _ClientId = Environment.GetEnvironmentVariable("GoogleClientId", EnvironmentVariableTarget.Machine)
                ?? throw new CustomException("Google ClientId not configured", StatusCodes.Status500InternalServerError);
        }

        public async Task<GoogleUserInfo?> ValidateGoogleTokenAsync(string idToken)
        {
            if (idToken == null)
            {
                _logger.LogInformation($"ID token => {idToken} validation returned null from Google server");
                throw new CustomException("ID token cannot be null", StatusCodes.Status400BadRequest);
            }

            var googleTokenInfoUrl = _configuration["Google:0AuthTokenUrl"] 
                    ?? throw new CustomException("Google TokenInfoUrl not configured", StatusCodes.Status500InternalServerError);
            var requestUrl = $"{googleTokenInfoUrl}{idToken}";
            var tokenInfo = await _networkService.GetAsync<GoogleTokenInfo>(requestUrl, null);
            if (tokenInfo == null || tokenInfo.Aud != _ClientId)
            {
                _logger.LogInformation($"ClientId returned for IdToken => {idToken} validation returned from Google server does not match");
                return null;
            }

            _logger.LogInformation($"User with Email => {tokenInfo.Email} validation from Google successful");

            return new GoogleUserInfo
            {
                Sub = tokenInfo.Sub,
                Email = tokenInfo.Email,
                EmailVerified = tokenInfo.EmailVerified,
                GivenName = tokenInfo.GivenName,
                FamilyName = tokenInfo.FamilyName,
                Picture = tokenInfo.Picture
            };
        }
    }
}
