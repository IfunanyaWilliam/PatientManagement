
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Enums;
    using Domain.Professional;

    public interface IProfessionalRepository
    {
        Task<Professional> CreateProfessionalAsync(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            string? qualification,
            string? license,
            UserRole userRole,
            CancellationToken cancellationToken = default);

        Task<Professional> ApproveProfessionalStatusAsync(
            Guid professionalId);

    }
}
