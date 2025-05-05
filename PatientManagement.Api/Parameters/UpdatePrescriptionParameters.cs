
namespace PatientManagement.Api.Parameters
{
    using Models;

    public class UpdatePrescriptionParameters
    {
        public UpdatePrescriptionParameters(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string? diagnosis,
            IEnumerable<PrescriptionMedicationDto>? medications)
        {
            PrescriptionId = prescriptionId;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Symptoms = symptoms;
            Diagnosis = diagnosis;
            Medications = medications;
        }


        public Guid PrescriptionId { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? ReasonForUpdate { get; set; }
        public IEnumerable<PrescriptionMedicationDto>? Medications { get; set; }
    }
}
