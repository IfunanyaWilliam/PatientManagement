
namespace PatientManagement.Common.Results
{
    using Common.Dto;
    public class GetAllPrescriptionsResult
    {
        public GetAllPrescriptionsResult(IEnumerable<GetPrescriptionResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<GetPrescriptionResult> Prescriptions { get; }
    }
}
