
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Results;
    using PatientManagement.Common.Dto;
    using PatientManagement.Common.Parameters;
    using PatientManagement.Domain.Prescription;

    public interface IPrescriptionRepository
    {
        Task<Prescription> CreatePrescriptionAsync(
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string diagnosis,
            IEnumerable<MedicationParameters> medications,
            CancellationToken cancellationToken);

        Task<Prescription> UpdatePrescriptionAsync(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string diagnosis,
            IEnumerable<MedicationParameters> medications,
            CancellationToken cancellationToken);

        Task<PrescriptionMedication> GetPrescriptionByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<IEnumerable<PrescriptionMedication>> GetPrescriptionsByPatientIdAsync(
            Guid patientId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<IEnumerable<PrescriptionMedication>> GetPrescriptionByProfessionalIdAsync(
            Guid professionalId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<IEnumerable<PrescriptionMedication>> GetAllPrescriptionsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken);
    }
}
