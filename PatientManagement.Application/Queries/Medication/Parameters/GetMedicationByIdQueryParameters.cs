

namespace PatientManagement.Application.Queries.Medication.Parameters
{
    using Interfaces.Queries;

    public class GetMedicationByIdQueryParameters : IQueryParameters
    {
        public GetMedicationByIdQueryParameters(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
