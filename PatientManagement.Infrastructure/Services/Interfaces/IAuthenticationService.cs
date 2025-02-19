
namespace PatientManagement.Infrastructure.Services.Interfaces
{
    using Common.Results;

    public interface IAuthenticationService
    {
        Task<AuthenticationResult> GetAuthTokenAsync(string email, string password);

        Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);

        Task InvalidateRefreshTokenAsync(string refreshToken);
    }
}
