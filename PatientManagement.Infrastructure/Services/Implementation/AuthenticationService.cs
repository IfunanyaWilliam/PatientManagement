
namespace PatientManagement.Infrastructure.Services.Implementation
{
    using System.Text;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using System.Security.Cryptography;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Entities;
    using Interfaces;
    using Common.Results;
    using PolicyProvider;
    using Common.Utilities;
    using Repositories.Interfaces;
    using Microsoft.Extensions.Logging;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _refreshTokenRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<AuthenticationService> _logger;


        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ITokenRepository refreshTokenRepository,
            IPermissionRepository permissionRepository,
            IEncryptionService encryptionService,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _permissionRepository = permissionRepository;
            _encryptionService = encryptionService;
            _logger = logger;
        }
        
        public async Task<AuthenticationResult> GetAuthTokenAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new CustomException($"Invalid credentials", StatusCodes.Status401Unauthorized);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await GenerateAccessToken(user, roles);
            var refreshToken = await GenerateRefreshToken(user);

            if(refreshToken is null)
            {
                return null;
            }

            return new AuthenticationResult(
                accessToken: accessToken,
                refreshToken: refreshToken);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || storedToken.IsRevoked || DateTime.UtcNow > storedToken.ExpiresAt)
            {
                _logger.LogInformation($"Refresh token for userId {storedToken.ApplicationUser} was invalid");
                throw new CustomException($"Invalid credentials", StatusCodes.Status400BadRequest);
            }

            var user = storedToken.ApplicationUser;
            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = await GenerateAccessToken(user, roles);
            var newRefreshToken = await GenerateRefreshToken(user);

            storedToken.IsRevoked = true;
            storedToken.DateModified = DateTime.UtcNow;
            var result = await _refreshTokenRepository.UpdateAsync(storedToken);

            if(!result)
                return null;

            return new AuthenticationResult(
                accessToken: accessToken,
                refreshToken: newRefreshToken);
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken != null)
            {
                storedToken.IsRevoked = true;
                storedToken.DateModified = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(storedToken);
            }
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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

        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var refreshTokenString = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshToken
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
