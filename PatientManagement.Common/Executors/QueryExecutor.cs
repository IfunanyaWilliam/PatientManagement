using Microsoft.Extensions.DependencyInjection;
using PatientManagement.Common.Handlers;

namespace PatientManagement.Common.Executors
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _container;

        public QueryExecutor(IServiceProvider container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task<TResult> ExecuteAsync<TParameters, TResult>(
            TParameters parameters,
            CancellationToken ct = default(CancellationToken))
            where TParameters : IQueryParameters
            where TResult : IQueryResult
        {
            ct.ThrowIfCancellationRequested();

            var handler = _container.GetRequiredService<IQueryHandler<TParameters, TResult>>();

            return await handler.HandleAsync(parameters, ct);
        }
    }
}
