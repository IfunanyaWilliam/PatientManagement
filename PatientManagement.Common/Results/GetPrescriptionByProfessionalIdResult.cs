
namespace PatientManagement.Common.Results
{
    public class GetPrescriptionByProfessionalIdResult
    {

        public GetPrescriptionByProfessionalIdResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
