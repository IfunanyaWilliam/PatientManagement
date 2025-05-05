

namespace PatientManagement.Domain.Prescription
{
    public class PrescriptionMedication
    {
        public PrescriptionMedication(
            Guid id,
            Guid patientId,
            Guid professionalId,
            Guid prescriptionId,
            string symptoms,
            string? diagnosis,
            IEnumerable<PrescribedMedication>? medications,
            bool isActive,
            DateTime dateCreated,
            DateTime? dateModified)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            PrescriptionId = prescriptionId;
            Symptoms = symptoms;
            Diagnosis = diagnosis;
            Medications = medications;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }


        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public Guid PrescriptionId { get; set; }
        public string Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public IEnumerable<PrescribedMedication>? Medications { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } 
        public DateTime? DateModified { get; set; }
    }
}
