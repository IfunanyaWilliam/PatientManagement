
namespace PatientManagement.Application.Interfaces.Handlers
{
    using Commands;

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
