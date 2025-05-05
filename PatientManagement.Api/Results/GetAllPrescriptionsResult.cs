namespace PatientManagement.Api.Results
{
    public class GetAllPrescriptionsResult
    {
        public GetAllPrescriptionsResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
