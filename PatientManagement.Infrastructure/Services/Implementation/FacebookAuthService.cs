

namespace PatientManagement.Infrastructure.Services.Implementation
{
    using Application.Interfaces.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Application.Utilities;
    using Domain.Authentication;


    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly ILogger<FacebookAuthService> _logger;
        private readonly INetworkService _networkService;
        private readonly IConfiguration _configuration;
        private readonly string _ClientId;
        private readonly string _ClientSecret;
        private readonly string _debugUrl;
        private readonly string _userInfoUrl;

        public FacebookAuthService(ILogger<FacebookAuthService> logger, INetworkService networkService, IConfiguration configuration)
        {
            _logger = logger;
            _networkService = networkService;
            _configuration = configuration;
            _ClientId = Environment.GetEnvironmentVariable("FacebookClientId", EnvironmentVariableTarget.Machine)
                ?? throw new CustomException("Facebook AppId not configured", StatusCodes.Status500InternalServerError);
            _ClientSecret = Environment.GetEnvironmentVariable("FacebookClientSecret", EnvironmentVariableTarget.Machine)
                ?? throw new CustomException("Facebook AppSecret not configured", StatusCodes.Status500InternalServerError);
            _debugUrl = _configuration["Facebook:GraphDebugTokenUrl"] 
                ?? throw new CustomException("Facebook GraphDebugTokenUrl not configured", StatusCodes.Status500InternalServerError);
            _userInfoUrl = _configuration["Facebook:GraphUserInfoUrl"] 
                ?? throw new CustomException("Facebook GraphUserInfoUrl not configured", StatusCodes.Status500InternalServerError);

        }


        public async Task<FacebookUserInfo?> VerifyFacebookTokenAsync(string accessToken)
        {
            if (accessToken == null)
            {
                throw new CustomException("Access token cannot be null", StatusCodes.Status400BadRequest);
            }

            var  debugUrl = $"{_debugUrl}{accessToken}&access_token={_ClientId}|{_ClientSecret}";
            
            var result = await _networkService.GetAsync<FacebookDebugTokenResponse>(debugUrl, null);

            if (result?.Data?.IsValid != true || result.Data.AppId != _ClientId) { return null; }
                
            var userInfoUrl = $"{_userInfoUrl}{accessToken}";
            var userInfo = await _networkService.GetAsync<FacebookUserInfo>(userInfoUrl, null);
            return userInfo;
        }
    }
}
