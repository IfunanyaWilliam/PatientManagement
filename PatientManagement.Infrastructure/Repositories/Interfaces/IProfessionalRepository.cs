
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using System.Threading.Tasks;
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

        Task<Professional> GetProfessionalByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<IEnumerable<Professional>> GetAllProfessionalsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken);
    }
}
