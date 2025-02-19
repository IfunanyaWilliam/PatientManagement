
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Common.Contracts;
    using Common.Dto;

    public class GetPrescriptionByProfessionalIdQueryResult : IQueryResult
    {
        public GetPrescriptionByProfessionalIdQueryResult(IEnumerable<PrescriptionDto> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<PrescriptionDto> Prescriptions { get; }
    }
}
