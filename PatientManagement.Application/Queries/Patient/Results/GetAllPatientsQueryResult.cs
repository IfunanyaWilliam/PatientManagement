
namespace PatientManagement.Application.Queries.Patient.Results
{
    using Common.Results;
    using Common.Contracts;

    public class GetAllPatientsQueryResult : IQueryResult
    {
        public GetAllPatientsQueryResult(IEnumerable<GetPatientResult> patients)
        {
            Patients = patients;
        }

        public IEnumerable<GetPatientResult>? Patients { get; set; }
    }
}
