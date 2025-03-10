
namespace PatientManagement.Application.Commands.Prescription.Parameters
{
    using PatientManagement.Common.Contracts;
    using PatientManagement.Common.Parameters;

    public class CreatePrescriptionCommandParameters : ICommand
    {
        public CreatePrescriptionCommandParameters(
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            IEnumerable<MedicationParameters>? medications)
        {
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
        }


        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Diagnosis { get; set; }
        public IEnumerable<MedicationParameters>? Medications { get; set; }
    }
}
