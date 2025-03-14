
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Results;
    using Domain.Patient;

    public interface IPatientRepository
    {
        Task<Patient> CreatePatientAsync(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            CancellationToken cancellationToken = default);

        Task<Patient> UpdatePatientAsync(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            CancellationToken cancellationToken = default);

        Task<Patient> GetPatientAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Patient>> GetAllPatientsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken = default);

        Task<bool> DeletePatientAsync(Guid id);
    }
}
