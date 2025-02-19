

namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Results;
    using Common.Enums;

    public interface IAccountRepository
    {

        Task<CreateUserResult> CreateUserAsync(
            string email,
            string password,
            UserRole role,
            CancellationToken cancellationToken = default);


    }
}
