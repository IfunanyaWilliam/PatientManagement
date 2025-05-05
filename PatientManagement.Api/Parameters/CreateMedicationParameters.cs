namespace PatientManagement.Api.Parameters
{
    public class CreateMedicationParameters
    {
        public CreateMedicationParameters(
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
