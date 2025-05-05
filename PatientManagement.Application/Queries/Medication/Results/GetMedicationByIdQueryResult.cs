
namespace PatientManagement.Application.Queries.Medication.Results
{
    using Interfaces.Queries;

    public class GetMedicationByIdQueryResult : IQueryResult
    {
        public GetMedicationByIdQueryResult(
            Guid id,
            string? name,
            string? description,
            bool isActive,
            DateTime dateCreated,
            DateTime? dateModified)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
