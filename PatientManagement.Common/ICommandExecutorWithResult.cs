﻿
namespace PatientManagement.Common
{
    public interface ICommandExecutorWithResult
    {
        Task<TResult> ExecuteAsync<TCommand, TResult>(
            TCommand command,
            CancellationToken ct = default(CancellationToken))
            where TCommand : ICommand
            where TResult : ICommandResult;
    }
}
