
namespace PatientManagement.Application.Queries.Prescription.Parameters
{
    using Common.Contracts;

    public class GetPrescriptionByPatientIdQueryParameters : IQueryParameters
    {
        public GetPrescriptionByPatientIdQueryParameters(
            Guid patientId,
            int pageNumber,
            int pageSize)
        {
            PatientId = patientId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; }

        public int PageSize { get; }

        public Guid PatientId { get; }
    }
}
