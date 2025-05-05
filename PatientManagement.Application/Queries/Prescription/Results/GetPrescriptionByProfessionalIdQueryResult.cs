
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Interfaces.Queries;

    public class GetPrescriptionByProfessionalIdQueryResult : IQueryResult
    {
        public GetPrescriptionByProfessionalIdQueryResult(IEnumerable<GetPrescriptionQueryResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionQueryResult> Prescriptions { get; }
    }
}
