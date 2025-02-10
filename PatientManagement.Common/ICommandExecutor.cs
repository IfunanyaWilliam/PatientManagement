
namespace PatientManagement.Common
{
    public interface ICommandExecutor
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
