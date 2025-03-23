
namespace PatientManagement.Application.Queries.Medication.Results
{
    using Common.Contracts;


    public class GetAllMedicationsQueryResult : IQueryResult
    {
        public GetAllMedicationsQueryResult(
            IEnumerable<GetMedicationsQueryResult> medications)
        {
            Medications = medications;
        }

        public IEnumerable<GetMedicationsQueryResult>? Medications { get; }
    }


    public class GetMedicationsQueryResult
    {
        public GetMedicationsQueryResult(
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
