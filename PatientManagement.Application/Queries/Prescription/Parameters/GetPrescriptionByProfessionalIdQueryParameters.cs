
namespace PatientManagement.Application.Queries.Prescription.Parameters
{
    using Common.Contracts;

    public class GetPrescriptionByProfessionalIdQueryParameters : IQueryParameters
    {
        public GetPrescriptionByProfessionalIdQueryParameters(
            Guid professionalId,
            int pageNumber,
            int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            ProfessionalId = professionalId;
        }

        public int PageNumber { get; }

        public int PageSize { get; }

        public Guid ProfessionalId { get; }
    }
}
