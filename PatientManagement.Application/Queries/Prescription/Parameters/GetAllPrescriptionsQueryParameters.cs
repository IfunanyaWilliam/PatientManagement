
namespace PatientManagement.Application.Queries.Prescription.Parameters
{
    using Common.Contracts;

    public class GetAllPrescriptionsQueryParameters : IQueryParameters
    {
        public GetAllPrescriptionsQueryParameters(
            int pageNumber,
            int pageSize,
            string searchParam)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchParam = searchParam;
        }

        public int PageNumber { get; }

        public int PageSize { get; }

        public string SearchParam { get; }
    }
}
