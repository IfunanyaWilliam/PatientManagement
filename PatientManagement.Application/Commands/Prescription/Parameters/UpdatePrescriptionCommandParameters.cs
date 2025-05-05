
namespace PatientManagement.Application.Commands.Prescription.Parameters
{
    using Interfaces.Commands;
    using Queries.Prescription.Dto;

    public class UpdatePrescriptionCommandParameters : ICommand
    {
        public UpdatePrescriptionCommandParameters(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string? diagnosis,
            IEnumerable<MedicationParameters>? medications)
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
        public IEnumerable<MedicationParameters>? Medications { get; set; }
    }
}
