
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Common.Contracts;
    using Common.Dto;
    using PatientManagement.Common.Results;

    public class GetAllPrescriptionsQueryResult : IQueryResult
    {
        public GetAllPrescriptionsQueryResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
