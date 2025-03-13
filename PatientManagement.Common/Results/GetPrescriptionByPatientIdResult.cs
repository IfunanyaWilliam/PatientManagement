
namespace PatientManagement.Common.Results
{
    using Common.Dto;


    public class GetPrescriptionByPatientIdResult
    {
        public GetPrescriptionByPatientIdResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
