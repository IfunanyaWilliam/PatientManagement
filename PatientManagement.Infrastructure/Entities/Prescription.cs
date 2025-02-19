
namespace PatientManagement.Infrastructure.Entities
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
        public Guid ProfessionalId { get; set; }
        public Professional? Professional { get; set; }
        public string? Diagnosis { get; set; }
        public List<Medication>? Medications { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime DateModified { get; set; }
    }
}
