
namespace PatientManagement.Application.Queries.Prescription.Results
{
    using Interfaces.Queries;
    using Domain.Prescription;

    public class GetPrescriptionByPatientIdQueryResult : IQueryResult
    {
        public GetPrescriptionByPatientIdQueryResult(IEnumerable<GetPrescriptionQueryResult> prescriptions)
        {
            Prescriptions = prescriptions;
        }

    public IEnumerable<GetPrescriptionQueryResult> Prescriptions { get; }
    }

    public class GetPrescriptionQueryResult
    {
        public GetPrescriptionQueryResult(
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

        public Guid Id { get; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public Guid PrescriptionId { get; }
        public string Symptoms { get; }
        public string? Diagnosis { get; }
        public IEnumerable<PrescribedMedication>? Medications { get; }
        public bool IsActive { get; }
        public DateTime DateCreated { get; }
        public DateTime? DateModified { get; }
    }
}
