
namespace PatientManagement.Application.Commands.Prescription.Handler
{
    using Parameters;
    using Results;
    using Microsoft.AspNetCore.Http;
    using Interfaces.Repositories;
    using Utilities;
    using Interfaces.Handlers;

    public class UpdatePrescriptionCommandHandler :
        ICommandHandlerWithResult<UpdatePrescriptionCommandParameters, UpdatePrescriptionCommandResult>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public UpdatePrescriptionCommandHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }
    
        public async Task<UpdatePrescriptionCommandResult> HandleAsync(
            UpdatePrescriptionCommandParameters command, 
            CancellationToken ct = default)
        {
            if (command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _prescriptionRepository.UpdatePrescriptionAsync(
                prescriptionId: command.PrescriptionId,
                patientId: command.PatientId,
                professionalId: command.ProfessionalId,
                symptoms: command.Symptoms,
                diagnosis: command.Diagnosis,
                medications: command.Medications,
                cancellationToken: ct);

            return new UpdatePrescriptionCommandResult(
                id: result.Id,
                patientId: result.PatientId,
                professionalId: result.ProfessionalId,
                diagnosis: result.Diagnosis,
                isActive: result.IsActive,
                createdDate: result.DateCreated,
                dateModified: result.DateModified);
        }
    }
}
