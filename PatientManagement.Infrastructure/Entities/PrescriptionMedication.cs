
namespace PatientManagement.Infrastructure.Entities
{
    public class PrescriptionMedication
    {
        public Guid Id { get; set; }
        public Guid MedicationId { get; set; }
        public Medication Medication { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public Prescription Prescription { get; set; }
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string? Instruction { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime? DateModified { get; set; }
    }
}
