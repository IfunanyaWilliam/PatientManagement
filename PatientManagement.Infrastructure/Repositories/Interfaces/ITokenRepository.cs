
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Entities;

    public interface ITokenRepository
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task<bool> CreateAsync(RefreshToken refreshToken);
        Task<bool> UpdateAsync(RefreshToken refreshToken);
    }
}
