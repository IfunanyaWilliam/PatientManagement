
namespace PatientManagement.Application.Interfaces.Queries
{
    public interface IQueryExecutor
    {
        Task<TResult> ExecuteAsync<TParameters, TResult>(
            TParameters parameters,
            CancellationToken ct = default(CancellationToken))
            where TParameters : IQueryParameters
            where TResult : IQueryResult;
    }
}
