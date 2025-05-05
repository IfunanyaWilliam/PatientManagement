
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Interfaces.Queries;

    public class GetAllPrescriptionsQueryResult : IQueryResult
    {
        public GetAllPrescriptionsQueryResult(IEnumerable<GetPrescriptionQueryResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionQueryResult> Prescriptions { get; }
    }
}
