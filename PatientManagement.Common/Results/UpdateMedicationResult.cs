
namespace PatientManagement.Common.Results
{
    public class UpdateMedicationResult
    {
        public UpdateMedicationResult(
            Guid id,
            string name,
            string description,
            bool isActive,
            DateTime createdDate,
            DateTime? dateModified)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
