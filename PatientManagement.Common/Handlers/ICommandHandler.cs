
namespace PatientManagement.Common.Handlers
{
    using Contracts;

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
