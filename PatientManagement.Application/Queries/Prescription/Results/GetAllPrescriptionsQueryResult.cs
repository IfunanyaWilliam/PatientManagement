
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Common.Contracts;
    using Common.Dto;

    public class GetAllPrescriptionsQueryResult : IQueryResult
    {
        public GetAllPrescriptionsQueryResult(IEnumerable<PrescriptionDto> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<PrescriptionDto> Prescriptions { get; }
    }
}
