
namespace PatientManagement.Common.Results
{
    public class CreatePrescriptionResult
    {
        public CreatePrescriptionResult(
            Guid id,
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string? diagnosis,
            bool isActive,
            DateTime dateCreated)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Symptoms = symptoms;
            Diagnosis = diagnosis;
            IsActive = isActive;
            DateCreated = dateCreated;
        }

        public Guid Id { get; set; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public string Symptoms { get; }
        public string? Diagnosis { get; }
        public bool IsActive { get; }
        public DateTime DateCreated { get; }
    }
}
