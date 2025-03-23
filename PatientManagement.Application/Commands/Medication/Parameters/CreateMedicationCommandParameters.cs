
namespace PatientManagement.Application.Commands.Medication.Parameters
{
    using Common.Contracts;

    public class CreateMedicationCommandParameters : ICommand
    {
        public CreateMedicationCommandParameters(
            string name,
            string description) 
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
