
namespace PatientManagement.Infrastructure.Repositories.Interfaces
{
    using Common.Results;
    using PatientManagement.Common.Dto;

    public interface IPrescriptionRepository
    {
        Task<CreatePrescriptionResult> CreatePrescriptionAsync(
            Guid patientId,
            Guid professionalId,
            string diagnosis,
            List<PrescriptionMedicationDto> medications,
            CancellationToken cancellationToken);

        Task<UpdatePrescriptionResult> UpdatePrescriptionAsync(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string diagnosis,
            List<PrescribedMedication> medications,
            CancellationToken cancellationToken);

        Task<GetPrescriptionByIdResult> GetPrescriptionByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<GetPrescriptionByPatientIdResult> GetPrescriptionByPatientIdAsync(
            Guid patientId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<GetPrescriptionByProfessionalIdResult> GetPrescriptionByProfessionalIdAsync(
            Guid professionalId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<GetAllPrescriptionsResult> GetAllPrescriptionsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken);
    }
}
