
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using DbContexts;
    using Application.Interfaces.Repositories;
    using Domain.RefreshToken;
    using Domain.ApplicationUser;

    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _dbContext;

        public TokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var query = await _dbContext.RefreshTokens.AsQueryable()
                                .Include(rt => rt.ApplicationUser)
                                .FirstOrDefaultAsync(rt => rt.Token == token);

            var refreshToken = new RefreshToken(
                    id: query.Id,
                    user: new ApplicationUser(
                        id: query.ApplicationUser.Id,
                        email: query.ApplicationUser.Email,
                        createdDate: query.ApplicationUser.DateCreated,
                        dateModified: query.ApplicationUser.DateModified,
                        isDeleted: query.ApplicationUser.IsDeleted,
                        userRole: query.ApplicationUser.Role),
                    token: query.Token,
                    isRevoked: query.IsRevoked,
                    expiresAt: query.ExpiresAt,
                    dateCreated: query.DateCreated,
                    dateModified: query.DateModified); 

            return refreshToken;
        }

        public async Task<bool> CreateAsync(RefreshTokenDto refreshToken)
        {
            var newRefreshToken = new Entities.RefreshToken
            {
                ApplicationUserId = refreshToken.ApplicationUserId,
                Token = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresAt
            };

            await _dbContext.RefreshTokens.AddAsync(newRefreshToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (existingToken != null)
            {
                existingToken.IsRevoked = true;
                existingToken.DateModified = DateTime.UtcNow;
            }

            _dbContext.RefreshTokens.Update(existingToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
