
namespace PatientManagement.Common.Parameters
{
    using Common.Dto;

    public class UpdatePrescriptionParameters
    {
        public UpdatePrescriptionParameters(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            List<PrescribedMedication>? medications)
        {
            PrescriptionId = prescriptionId;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
        }

        
        public Guid PrescriptionId { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Diagnosis { get; set; }
        public List<PrescribedMedication>? Medications { get; set; }
    }
}
