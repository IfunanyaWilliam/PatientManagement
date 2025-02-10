﻿
namespace PatientManagement.Common.Handlers
{
    public interface ICommandHandlerWithResult<in TCommand, TResult>
        where TCommand : ICommand
        where TResult : ICommandResult
    {
        Task<TResult> HandleAsync(
            TCommand command,
            CancellationToken ct = default(CancellationToken));
    }
}
