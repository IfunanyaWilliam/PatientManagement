namespace PatientManagement.Api.Models
{
    public class PrescriptionMedicationDto
    {
        public PrescriptionMedicationDto(
            Guid medicationId,
            string? dosage,
            string? instruction)
        {
            MedicationId = medicationId;
            Dosage = dosage;
            Instruction = instruction;
        }

        public Guid MedicationId { get; set; }
        public string? Dosage { get; set; }
        public string? Instruction { get; set; }
    }
}
