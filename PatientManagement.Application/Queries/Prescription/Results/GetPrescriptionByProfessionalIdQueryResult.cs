
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Common.Contracts;
    using PatientManagement.Common.Results;

    public class GetPrescriptionByProfessionalIdQueryResult : IQueryResult
    {
        public GetPrescriptionByProfessionalIdQueryResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
