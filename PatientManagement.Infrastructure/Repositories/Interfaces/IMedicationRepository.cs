
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using PatientManagement.Domain.Prescription;


    public interface IMedicationRepository
    {
        Task<Medication> CreateMedicationAsync(
            string name,
            string description,
            CancellationToken cancellationToken);
    }
}
