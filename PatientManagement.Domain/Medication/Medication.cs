
namespace PatientManagement.Domain.Medication
{
    public class Medication
    {
        public Medication(
            Guid id,
            Guid patientId,
            Guid prescriptionId,
            Guid professionalId,
            string? name,
            string? dosage,
            bool isActive,
            bool isDeleted,
            DateTime createdDate,
            DateTime dateModified)
        {
            Id = id;
            PatientId = patientId;
            PrescriptionId = prescriptionId;
            ProfessionalId = professionalId;
            Name = name;
            Dosage = dosage;
            IsActive = isActive;
            IsDeleted = isDeleted;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid PrescriptionId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string? Name { get; set; }
        public string? Dosage { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateModified { get; set; }
    }
}
