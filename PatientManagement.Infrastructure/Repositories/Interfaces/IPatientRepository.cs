
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Results;

    public interface IPatientRepository
    {
        Task<CreatePatientResult> CreatePatientAsync(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            CancellationToken cancellationToken = default);

        Task<UpdatePatientResult> UpdatePatientAsync(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            CancellationToken cancellationToken = default);

        Task<GetPatientResult> GetPatientAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<bool> DeletePatientAsync(Guid id);
    }
}
