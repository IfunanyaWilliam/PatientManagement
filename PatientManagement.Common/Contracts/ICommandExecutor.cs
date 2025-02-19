
namespace PatientManagement.Common.Contracts
{
    public interface ICommandExecutor
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
