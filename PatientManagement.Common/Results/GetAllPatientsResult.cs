
namespace PatientManagement.Common.Results
{
    public class GetAllPatientsResult
    {
        public GetAllPatientsResult(IEnumerable<GetPatientResult> patients)
        {
            Patients = patients;
        }

        public IEnumerable<GetPatientResult>? Patients { get; set; }
    }
}
