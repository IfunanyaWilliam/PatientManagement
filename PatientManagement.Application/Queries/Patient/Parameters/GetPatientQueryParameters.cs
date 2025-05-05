
namespace PatientManagement.Application.Queries.Patient.Parameters
{
    using Interfaces.Queries;

    public class GetPatientQueryParameters : IQueryParameters
    {
        public GetPatientQueryParameters(Guid patientId)
        {
            PatientId = patientId;
        }

        public Guid PatientId { get; }
    }
}
