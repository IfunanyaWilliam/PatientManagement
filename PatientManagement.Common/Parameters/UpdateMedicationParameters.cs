
namespace PatientManagement.Common.Parameters
{
    public class UpdateMedicationParameters
    {
        public UpdateMedicationParameters(
            Guid medicationId,
            string name,
            string description)
        {
            MedicationId = medicationId;
            Name = name;
            Description = description;
        }

        public Guid MedicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
