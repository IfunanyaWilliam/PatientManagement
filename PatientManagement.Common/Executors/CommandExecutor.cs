﻿
namespace PatientManagement.Common.Executors
{
    using Microsoft.Extensions.DependencyInjection;
    using Handlers;
    using Contracts;


    public class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _container;

        public CommandExecutor(IServiceProvider container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _container.GetRequiredService<ICommandHandler<TCommand>>();

            await handler.HandleAsync(command);
        }
    }
}
