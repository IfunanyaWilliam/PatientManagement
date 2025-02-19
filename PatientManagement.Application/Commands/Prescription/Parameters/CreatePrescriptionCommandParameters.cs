
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
