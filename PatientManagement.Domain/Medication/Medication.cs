
namespace PatientManagement.Domain.Medication
{
    public class Medication
    {
        public Medication(
            Guid id,
            string? name,
            string? description,
            bool isActive,
            bool isDeleted,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            IsDeleted = isDeleted;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
    }
}
