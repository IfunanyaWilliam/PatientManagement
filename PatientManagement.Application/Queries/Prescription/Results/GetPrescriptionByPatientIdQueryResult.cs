
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Common.Dto;
    using Common.Contracts;

    public class GetPrescriptionByPatientIdQueryResult : IQueryResult
    {
        public GetPrescriptionByPatientIdQueryResult(IEnumerable<PrescriptionDto> prescriptions)
        {
            Prescriptions = prescriptions;
        }

    public IEnumerable<PrescriptionDto> Prescriptions { get; }
}
}
