
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Common.Contracts;
    using PatientManagement.Common.Results;

    public class GetPrescriptionByPatientIdQueryResult : IQueryResult
    {
        public GetPrescriptionByPatientIdQueryResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

    public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
}
}
