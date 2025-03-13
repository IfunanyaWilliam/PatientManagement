
namespace PatientManagement.Application.Commands.Prescription.Handler
{
    using Microsoft.AspNetCore.Http;
    using Infrastructure.Repositories.Interfaces;
    using Parameters;
    using Results;
    using Common.Handlers;
    using Common.Utilities;
    using Common.Parameters;

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

            if(command.Medications is null)
                throw new CustomException($"Invalid input: Medication is null", StatusCodes.Status400BadRequest);

            var result = await _prescriptionRepository.CreatePrescriptionAsync(
                patientId: command.PatientId,
                professionalId: command.ProfessionalId,
                symptoms: command.Symptoms,
                diagnosis: command.Diagnosis,
                medications: command.Medications?.Select(m => new MedicationParameters(
                    medicationId: m.MedicationId,
                    dosage: m.Dosage,
                    instruction: m.Instruction)),
                cancellationToken: ct);

            return new CreatePrescriptionCommandResult(
                id: result.Id,
                patientId: result.PatientId,
                professionalId: result.ProfessionalId,
                symptoms: result.Symptoms,
                diagnosis: result.Diagnosis,
                isActive: result.IsActive,
                dateCreated: result.DateCreated);
        }
    }
}
