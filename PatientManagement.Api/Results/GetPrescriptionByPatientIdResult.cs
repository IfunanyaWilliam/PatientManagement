namespace PatientManagement.Api.Results
{
    public class GetPrescriptionByPatientIdResult
    {
        public GetPrescriptionByPatientIdResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
