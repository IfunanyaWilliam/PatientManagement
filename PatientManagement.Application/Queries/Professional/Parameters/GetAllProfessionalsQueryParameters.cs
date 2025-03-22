
namespace PatientManagement.Application.Queries.Professional.Parameters
{
    using Common.Contracts;

    public class GetAllProfessionalsQueryParameters : IQueryParameters
    {
        public GetAllProfessionalsQueryParameters(
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
