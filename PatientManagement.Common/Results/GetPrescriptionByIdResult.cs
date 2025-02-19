
namespace PatientManagement.Common.Results
{
    using Dto;

    public class GetPrescriptionByIdResult
    {
        public GetPrescriptionByIdResult(
            Guid id,
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            List<PrescribedMedication>? medications,
            bool isActive,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
            IsActive = isActive;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public string? Diagnosis { get; }
        public List<PrescribedMedication>? Medications { get; }
        public bool IsActive { get; }
        public DateTime CreatedDate { get; }
        public DateTime DateModified { get; }
    }
}
