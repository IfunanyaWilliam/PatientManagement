
namespace PatientManagement.Api.Parameters
{
    using Models;

    public class CreatePrescriptionParameters
    {
        public CreatePrescriptionParameters()
        {
        }

        public CreatePrescriptionParameters(
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string? diagnosis,
            List<PrescriptionMedicationDto>? medications)
        {
            PatientId = patientId;
            ProfessionalId = professionalId;
            Symptoms = symptoms;
            Diagnosis = diagnosis;
            Medications = medications;
        }


        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public List<PrescriptionMedicationDto>? Medications { get; set; }
    }
}
