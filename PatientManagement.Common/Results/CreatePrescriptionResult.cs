
namespace PatientManagement.Common.Results
{
    using Common.Dto;

    public class CreatePrescriptionResult
    {
        public CreatePrescriptionResult(
            Guid id,
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            bool isActive,
            DateTime dateCreated)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            IsActive = isActive;
            DateCreated = dateCreated;
        }

        public Guid Id { get; set; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public string? Diagnosis { get; }
        public bool IsActive { get; }
        public DateTime DateCreated { get; }
    }
}
