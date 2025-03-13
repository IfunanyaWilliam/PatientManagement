
namespace PatientManagement.Common.Results
{
    using Common.Dto;

    public class GetPrescriptionByProfessionalIdResult
    {

        public GetPrescriptionByProfessionalIdResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
