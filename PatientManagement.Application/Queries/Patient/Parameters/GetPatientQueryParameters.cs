
namespace PatientManagement.Application.Queries.Patient.Parameters
{
    using Common.Contracts;
    public class GetPatientQueryParameters : IQueryParameters
    {
        public GetPatientQueryParameters(Guid patientId)
        {
            PatientId = patientId;
        }

        public Guid PatientId { get; }
    }
}
