
namespace PatientManagement.Application.Commands.Prescription.Parameters
{
    using Interfaces.Commands;
    using Queries.Prescription.Dto;

    public class CreatePrescriptionCommandParameters : ICommand
    {
        public CreatePrescriptionCommandParameters(
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string? diagnosis,
            IEnumerable<MedicationParameters>? medications)
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
        public IEnumerable<MedicationParameters>? Medications { get; set; }
    }
}
