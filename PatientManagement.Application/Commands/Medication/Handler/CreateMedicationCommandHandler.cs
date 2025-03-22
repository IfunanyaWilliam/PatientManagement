
namespace PatientManagement.Application.Commands.Medication.Handler
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Results;
    using Parameters;
    using Common.Utilities;
    using Common.Handlers;
    using Infrastructure.Repositories.Interfaces;
   

    public class CreateMedicationCommandHandler :
        ICommandHandlerWithResult<CreateMedicationCommandParameters, CreateMedicationCommandResult>
    {
        private readonly IMedicationRepository _medicationRepository;

        public CreateMedicationCommandHandler(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<CreateMedicationCommandResult> HandleAsync(
            CreateMedicationCommandParameters command, 
            CancellationToken ct = default)
        {
            if(command == null)
            {
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);
            }

            var result = await _medicationRepository.CreateMedicationAsync(
                name: command.Name,
                description: command.Description,
                cancellationToken: ct);

            return new CreateMedicationCommandResult(
                id: result.Id,
                name: result.Name,
                description: result.Description,
                isActive: result.IsActive,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified);
        }
    }
}
