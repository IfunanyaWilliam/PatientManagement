
namespace PatientManagement.Common.Results
{
    using Common.Dto;


    public class GetPrescriptionByPatientIdResult
    {
        public GetPrescriptionByPatientIdResult(IEnumerable<PrescriptionDto> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<PrescriptionDto> Prescriptions { get; }
    }
}
