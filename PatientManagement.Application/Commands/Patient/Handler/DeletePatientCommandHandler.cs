
namespace PatientManagement.Application.Commands.Patient.Handler
{
    using Parameters;
    using Results;
    using Microsoft.Extensions.Logging;
    using Interfaces.Repositories;
    using Interfaces.Handlers;

    public class DeletePatientCommandHandler :
        ICommandHandlerWithResult<DeletePatientCommandParameters, DeletePatientCommandResult>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<DeletePatientCommandHandler> _logger;

        public DeletePatientCommandHandler(
            IPatientRepository patientRepository,
            ILogger<DeletePatientCommandHandler> logger) 
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<DeletePatientCommandResult> HandleAsync(
            DeletePatientCommandParameters command, 
            CancellationToken ct = default)
        {
            bool isDeleted = await _patientRepository.DeletePatientAsync(command.Id);

            if (!isDeleted)
            {
                _logger.LogError("Internal Server Error {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {command.Id}",
                    "DeletePatientCommandHandler: Patient not deleted",
                    DateTime.UtcNow.AddHours(1));
            }

            return new DeletePatientCommandResult(isDeleted);
        }
    }
}
