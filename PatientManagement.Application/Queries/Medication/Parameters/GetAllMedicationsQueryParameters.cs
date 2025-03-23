
namespace PatientManagement.Application.Queries.Medication.Parameters
{
    using Common.Contracts;

    public class GetAllMedicationsQueryParameters : IQueryParameters
    {
        public GetAllMedicationsQueryParameters(
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
