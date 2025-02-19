
namespace PatientManagement.Application.Commands.Prescription.Handler
{
    using Microsoft.AspNetCore.Http;
    using Infrastructure.Repositories.Interfaces;
    using Parameters;
    using Results;
    using Common.Handlers;
    using Common.Utilities;
    

    public class CreatePrescriptionCommandHandler :
        ICommandHandlerWithResult<CreatePrescriptionCommandParameters, CreatePrescriptionCommandResult>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public CreatePrescriptionCommandHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<CreatePrescriptionCommandResult> HandleAsync(
            CreatePrescriptionCommandParameters command, 
            CancellationToken ct = default)
        {
            if (command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _prescriptionRepository.CreatePrescriptionAsync(
                patientId: command.PatientId,
                professionalId: command.ProfessionalId,
                diagnosis: command.Diagnosis,
                medications: command.Medications,
                cancellationToken: ct);

            return new CreatePrescriptionCommandResult(
                id: result.Id,
                patientId: result.PatientId,
                professionalId: result.ProfessionalId,
                diagnosis: result.Diagnosis,
                medications: result.Medications,
                isActive: result.IsActive,
                createdDate: result.CreatedDate);
        }
    }
}
