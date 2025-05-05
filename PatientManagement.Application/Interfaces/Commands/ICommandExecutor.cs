
namespace PatientManagement.Application.Interfaces.Commands
{
    public interface ICommandExecutor
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
