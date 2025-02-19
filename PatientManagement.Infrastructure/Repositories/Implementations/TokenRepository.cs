
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using DbContexts;
    using Entities;
    using Interfaces;

    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _dbContext;

        public TokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var query = _dbContext.RefreshTokens.AsQueryable();
            query = query.Include(rt => rt.ApplicationUser);
            return await query.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<bool> CreateAsync(RefreshToken refreshToken)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Update(refreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
