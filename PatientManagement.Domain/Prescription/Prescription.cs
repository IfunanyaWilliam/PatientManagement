
namespace PatientManagement.Domain.Prescription
{
    public class Prescription
    {
        public Prescription(
            Guid id, 
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string? diagnosis,
            bool isActive,
            DateTime dateCreated,
            DateTime dateModified)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Symptoms = symptoms;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }


        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public string Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
