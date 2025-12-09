
namespace PatientManagement.Infrastructure.Services.Implementation
{
    using System.Text;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Security.Cryptography;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Application.Interfaces.Repositories;
    using Application.Interfaces.Services;
    using Application.Utilities;
    using Domain.Authentication;
    using Domain.RefreshToken;
    using PolicyProvider;
    using Interfaces;
    
    

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<Entities.ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _refreshTokenRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            UserManager<Entities.ApplicationUser> userManager,
            IConfiguration configuration,
            ITokenRepository refreshTokenRepository,
            IPermissionRepository permissionRepository,
            IEncryptionService encryptionService,
            ILogger<AuthenticationService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _permissionRepository = permissionRepository;
            _encryptionService = encryptionService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<AuthenticationResultDto> GetAuthTokenAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogInformation($"Authentication failed for user with email {email}");
                throw new CustomException($"Invalid credentials", StatusCodes.Status401Unauthorized);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await GenerateAccessToken(user, roles);
            var refreshToken = await GenerateRefreshToken(user);

            if(refreshToken is null)
            {
                return null;
            }

            return new AuthenticationResultDto(
                accessToken: accessToken,
                refreshToken: refreshToken);
        }

        public async Task<AuthResultDto> GetAuthTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"User with facebook email {email} was not found");   
                throw new CustomException($"Invalid credentials", StatusCodes.Status401Unauthorized);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await GenerateAccessToken(user, roles);
            var refreshToken = await GenerateRefreshToken(user);

            if (refreshToken is null)
            {
                return null;
            }

            return new AuthResultDto(
                userId: user.Id,
                accessToken: accessToken,
                refreshToken: refreshToken);
        }

        public async Task<AuthenticationResultDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || storedToken.IsRevoked || DateTime.UtcNow > storedToken.ExpiresAt)
            {
                _logger.LogInformation($"Refresh token for userId {storedToken.ApplicationUser} was invalid");
                throw new CustomException($"Invalid credentials", StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByEmailAsync(storedToken?.ApplicationUser?.Email);
            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = await GenerateAccessToken(user, roles);
            var newRefreshToken = await GenerateRefreshToken(user);

            storedToken.IsRevoked = true;
            storedToken.DateModified = DateTime.UtcNow;
            var result = await _refreshTokenRepository.RevokeRefreshTokenAsync(storedToken.Token);

            if(!result)
                return null;

            return new AuthenticationResultDto(
                accessToken: accessToken,
                refreshToken: newRefreshToken);
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            if (refreshToken != null)
            {
                await _refreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);
            }
        }

        private async Task<string> GenerateAccessToken(Entities.ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new("ipAddress", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString("D"))
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var roleIds = await _permissionRepository.GetRoleIdsByNamesAsync(roles);
            var permissions = await _permissionRepository.GetPermissionsByRoleIdsAsync(roleIds);

            foreach (var permission in permissions)
            {
                var decryptedPermission = _encryptionService.Decrypt(permission.EncryptedName);
                claims.Add(new Claim(AppClaimTypes.Permissions, decryptedPermission));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenDurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(Entities.ApplicationUser user)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var refreshTokenString = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshTokenDto
            {
                Token = refreshTokenString,
                ApplicationUserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenDurationInDays"]))
            };

            var result = await _refreshTokenRepository.CreateAsync(refreshToken);
            return result ? refreshTokenString : string.Empty;
        }
    }
}
