
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Enums;
    using Common.Results;


    public interface IProfessionalRepository
    {
        Task<CreateProfessionalResult> CreateProfessionalAsync(
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

        Task<ApproveProfessionalStatusResult> ApproveProfessionalStatusAsync(
            Guid professionalId);

    }
}
