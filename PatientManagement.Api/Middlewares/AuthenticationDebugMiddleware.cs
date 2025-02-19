
namespace PatientManagement.Api.Middlewares
{
    using System.Text;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Authorization;
    using Common.Utilities;


    public class AuthenticationDebugMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationDebugMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationDebugMiddleware(
            RequestDelegate next, 
            ILogger<AuthenticationDebugMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (RequiresAuthorization(context))
                    ProcessBearerToken(context);

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Process threw an exception");

                if (ex is CustomException customException || ex.Message != null)
                {
                    throw; 
                }
                _logger.LogError(ex, "Authentication process threw an exception");
                throw new CustomException($"Error occured while processing bearer token", StatusCodes.Status500InternalServerError);
            }
        }

        private bool RequiresAuthorization(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                return false;
            }

            var hasAuthorizeAttribute = endpoint.Metadata.Any(m => m is AuthorizeAttribute);
            var hasAllowAnonymousAttribute = endpoint.Metadata.Any(m => m is AllowAnonymousAttribute);

            if (hasAllowAnonymousAttribute)
            {
                return false;
            }

            return hasAuthorizeAttribute;
        }

        private void ProcessBearerToken(HttpContext context)
        {
            string token = ExtractBearerToken(context);
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogInformation("No bearer token found in request");
                throw new CustomException($"This request requires an access token", StatusCodes.Status401Unauthorized);
            }

            _logger.LogInformation("Bearer token found. Processing claims...");

            try
            {
                var claims = ExtractClaims(token);
                if (claims != null && claims.Any())
                {
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    var principal = new ClaimsPrincipal(identity);
                    context.User = principal;
                    _logger.LogInformation("Claims successfully extracted and assigned to context.User");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bearer token");
                throw new CustomException($"Error processing Bearer Token", StatusCodes.Status401Unauthorized);
            }
        }

        private string ExtractBearerToken(HttpContext context)
        {
            string authorizationHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        private IEnumerable<Claim> ExtractClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                    ClockSkew = TimeSpan.Zero
                };

                // Validate the token and extract claims
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                _logger.LogInformation("Token validation successful");
                return principal.Claims;
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning($"Token has expired : {ex.Message}");
                throw new CustomException($"Invalid Token", StatusCodes.Status401Unauthorized);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                _logger.LogWarning($"Token has invalid signature : {ex.Message}");
                throw new CustomException($"Invalid Token", StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting claims from token");
                throw new CustomException($"Invalid Token", StatusCodes.Status401Unauthorized);
            }
        }
    }
}
