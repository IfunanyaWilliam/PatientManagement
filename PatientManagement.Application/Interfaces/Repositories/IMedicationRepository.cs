
namespace PatientManagement.Application.Interfaces.Repositories
{
    using Domain.Prescription;

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

        Task<IEnumerable<Medication>> GetAllMedicationsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken);
    }
}
