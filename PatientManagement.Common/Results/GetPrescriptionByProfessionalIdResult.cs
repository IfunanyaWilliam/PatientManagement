
namespace PatientManagement.Common.Results
{
    using Common.Dto;

    public class GetPrescriptionByProfessionalIdResult
    {

        public GetPrescriptionByProfessionalIdResult(IEnumerable<PrescriptionDto> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<PrescriptionDto> Prescriptions { get; }
    }
}
