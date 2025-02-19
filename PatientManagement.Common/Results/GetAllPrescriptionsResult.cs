
namespace PatientManagement.Common.Results
{
    using Common.Dto;
    public class GetAllPrescriptionsResult
    {
        public GetAllPrescriptionsResult(IEnumerable<PrescriptionDto> prescriptions)
        {
            Prescriptions = prescriptions;
        }

        public IEnumerable<PrescriptionDto> Prescriptions { get; }
    }
}
