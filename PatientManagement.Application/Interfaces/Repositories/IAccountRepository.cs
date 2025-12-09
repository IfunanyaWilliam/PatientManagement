
namespace PatientManagement.Application.Interfaces.Repositories
{
    using Domain.ApplicationUser;
    using Domain.Account;

    public interface IAccountRepository
    {

        Task<CreateUserResultDto> CreateUserAsync(
            string email,
            string password,
            UserRole role,
            CancellationToken cancellationToken = default);

        Task<bool> InsertExternalLoginAsync(
            Guid userId,
            string provider,
            string providerUserId,
            CancellationToken cancellationToken = default);

    }
}
