
namespace PatientManagement.Common.Parameters
{
    public class MedicationParameters
    {
        public MedicationParameters(
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
