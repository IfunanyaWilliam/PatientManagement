namespace PatientManagement.Api.Results
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
