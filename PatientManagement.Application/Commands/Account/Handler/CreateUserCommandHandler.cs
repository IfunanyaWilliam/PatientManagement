
namespace PatientManagement.Application.Commands.Account.Handler
{
    using Paremeters;
    using Result;
    using Interfaces.Repositories;
    using Interfaces.Handlers;

    public class CreateUserCommandHandler :
        ICommandHandlerWithResult<CreateUserCommandParameters, CreateUserCommandResults>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateUserCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository
                                 ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<CreateUserCommandResults> HandleAsync(
            CreateUserCommandParameters command,
            CancellationToken ct = new CancellationToken())
        {
            if (string.IsNullOrEmpty(command.Email) || string.IsNullOrEmpty(command.Password))
                throw new ArgumentException("Invalid parameters");

            var result = await _accountRepository.CreateUserAsync(
                    email: command.Email,
                    password: command.Password,
                    role: command.UserRole,
                cancellationToken: ct);

            return new CreateUserCommandResults(
                userId: result.UserId,
                email: result.Email,
                userRole: result.UserRole,
                created: result.DateCreated);
        }
    }
}
