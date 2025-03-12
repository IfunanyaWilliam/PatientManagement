
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
            IEnumerable<PrescriptionMedicationDto>? medications)
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
        public string? ReasonForUpdate { get; set; }
        public IEnumerable<PrescriptionMedicationDto>? Medications { get; set; }
    }
}
