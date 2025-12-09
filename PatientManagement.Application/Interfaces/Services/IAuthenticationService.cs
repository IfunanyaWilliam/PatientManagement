
namespace PatientManagement.Application.Interfaces.Services
{
    using Domain.Authentication;

    public interface IAuthenticationService
    {
        Task<AuthenticationResultDto> GetAuthTokenAsync(string email, string password);

        Task<AuthResultDto> GetAuthTokenAsync(string email);

        Task<AuthenticationResultDto> RefreshTokenAsync(string refreshToken);

        Task InvalidateRefreshTokenAsync(string refreshToken);
    }
}
