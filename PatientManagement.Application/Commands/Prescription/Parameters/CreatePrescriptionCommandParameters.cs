
namespace PatientManagement.Application.Commands.Prescription.Parameters
{
    using PatientManagement.Common.Contracts;
    using PatientManagement.Common.Dto;

    public class CreatePrescriptionCommandParameters : ICommand
    {
        public CreatePrescriptionCommandParameters(
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            List<PrescriptionMedicationDto>? medications)
        {
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
        }


        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Diagnosis { get; set; }
        public List<PrescriptionMedicationDto>? Medications { get; set; }
    }
}
