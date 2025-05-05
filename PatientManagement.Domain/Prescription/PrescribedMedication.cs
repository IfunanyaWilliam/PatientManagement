
namespace PatientManagement.Domain.Prescription
{
    public class PrescribedMedication
    {
        public PrescribedMedication(
            Guid medicationId,
            string? name,
            string? dosage,
            bool isActive,
            string? instruction)
        {
            MedicationId = medicationId;
            Name = name;
            Dosage = dosage;
            IsActive = isActive;
            Instruction = instruction;
        }

        public Guid MedicationId { get; set; }
        public string? Name { get; set; }
        public string? Dosage { get; set; }
        public string? Instruction { get; set; }
        public bool IsActive { get; set; }
    }
}
