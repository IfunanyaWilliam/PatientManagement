
namespace PatientManagement.Application.Commands.Medication.Handler
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Infrastructure.Repositories.Interfaces;
    using Common.Utilities;
    using Common.Handlers;
    using Parameters;
    using Results;
    
    public class UpdateMedicationCommandHandler :
        ICommandHandlerWithResult<UpdateMedicationCommandParameters, UpdateMedicationCommandResult>
    {
        private readonly IMedicationRepository _medicationRepository;

        public UpdateMedicationCommandHandler(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<UpdateMedicationCommandResult> HandleAsync(
            UpdateMedicationCommandParameters command, 
            CancellationToken ct = default)
        {
            if (command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _medicationRepository.UpdateMedicationAsync(
                medicationId: command.MedicationId,
                name: command.Name,
                description: command.Description,
                cancellationToken: ct);

            return new UpdateMedicationCommandResult(
                id: result.Id,
                name: result.Name,
                description: result.Description,
                isActive: result.IsActive,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified);
        }
    }
}
