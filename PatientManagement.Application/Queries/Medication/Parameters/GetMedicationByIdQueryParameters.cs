

namespace PatientManagement.Application.Queries.Medication.Parameters
{
    using Common.Contracts;

    public class GetMedicationByIdQueryParameters : IQueryParameters
    {
        public GetMedicationByIdQueryParameters(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
