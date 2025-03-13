
namespace PatientManagement.Common.Results
{
    using Common.Dto;

    public class UpdatePrescriptionResult
    {
        public UpdatePrescriptionResult(
            Guid id,
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            bool isActive,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            IsActive = isActive;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public string? Diagnosis { get; }
        public bool IsActive { get; }
        public DateTime CreatedDate { get; }
        public DateTime DateModified { get; }
    }
}
