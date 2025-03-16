
namespace PatientManagement.Application.Queries.Patient.Parameters
{
    using Common.Contracts;

    public class GetAllPatientsQueryParameters : IQueryParameters
    {
        public GetAllPatientsQueryParameters(
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
