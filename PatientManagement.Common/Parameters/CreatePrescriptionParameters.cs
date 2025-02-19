
namespace PatientManagement.Common.Parameters
{
    using Common.Dto;

    public class CreatePrescriptionParameters
    {
        public CreatePrescriptionParameters()
        {
        }

        public CreatePrescriptionParameters(
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            List<PrescriptionMedication>? medications)
        {
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
        }


        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Diagnosis { get; set; }
        public List<PrescriptionMedication>? Medications { get; set; }
    }
}
