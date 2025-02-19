
namespace PatientManagement.Common.Dto
{
    public class PrescriptionMedication
    {
        public PrescriptionMedication(
            string? name,
            string? dosage)
        {
            Name = name;
            Dosage = dosage;
        }

        public string? Name { get; set; }
        public string? Dosage { get; set; }
    }
}
