﻿
namespace PatientManagement.Application.Interfaces.Repositories
{
    using Domain.ApplicationUser;
    using PatientManagement.Domain.Account;

    public interface IAccountRepository
    {

        Task<CreateUserResultDto> CreateUserAsync(
            string email,
            string password,
            UserRole role,
            CancellationToken cancellationToken = default);

    }
}
