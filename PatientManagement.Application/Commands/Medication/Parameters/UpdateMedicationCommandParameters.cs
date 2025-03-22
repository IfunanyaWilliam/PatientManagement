
namespace PatientManagement.Application.Commands.Medication.Parameters
{
    using Common.Contracts;

    public class UpdateMedicationCommandParameters : ICommand
    {
        public UpdateMedicationCommandParameters(
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
