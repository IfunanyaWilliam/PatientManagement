
namespace PatientManagement.Common.Dto
{
    public class PrescribedMedication
    {
        public PrescribedMedication(
            Guid? id,
            string? name,
            string? dosage,
            bool isActive)
        {
            Id = id;
            Name = name;
            Dosage = dosage;
            IsActive = isActive;
        }

        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Dosage { get; set; }
        public bool IsActive { get; set; }
    }
}
