
namespace PatientManagement.Application.Interfaces.Repositories
{
    using Domain.RefreshToken;

    public interface ITokenRepository
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task<bool> CreateAsync(RefreshTokenDto refreshToken);
        Task<bool> UpdateAsync(string refreshToken);
    }
}
