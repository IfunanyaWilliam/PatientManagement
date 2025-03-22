
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using PatientManagement.Domain.Prescription;


    public interface IMedicationRepository
    {
        Task<Medication> CreateMedicationAsync(
            string name,
            string description,
            CancellationToken cancellationToken);

        Task<Medication> UpdateMedicationAsync(
            Guid medicationId,
            string name,
            string description,
            CancellationToken cancellationToken);

        Task<Medication> GetMedicationByIdAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}
