
namespace PatientManagement.Domain.Prescription
{
    public class Prescription
    {
        public Prescription(
            Guid id, 
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            string? medications,
            bool isActive,
            bool isDeleted,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
            IsActive = isActive;
            IsDeleted = isDeleted;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }


        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Medications { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
    }
}
