
namespace PatientManagement.Common.Handlers
{
    public interface IQueryHandler<in TParameters, TResult>
        where TParameters : IQueryParameters
        where TResult : IQueryResult
    {
        Task<TResult> HandleAsync(
            TParameters parameters,
            CancellationToken ct = default(CancellationToken));
    }
}
