
namespace PatientManagement.Application.Commands.Patient.Handler
{
    using Parameters;
    using Results;
    using Common.Handlers;
    using Infrastructure.Repositories.Interfaces;
    using PatientManagement.Common.Utilities;
    using Microsoft.AspNetCore.Http;

    public class UpdatePatientCommandHandler : 
        ICommandHandlerWithResult<UpdatePatientCommandParameters, UpdatePatientCommandResult>
    {

        private readonly IPatientRepository _patientRepository;

        public UpdatePatientCommandHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<UpdatePatientCommandResult> HandleAsync(
            UpdatePatientCommandParameters command, 
            CancellationToken ct = default)
        {
            if (command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _patientRepository.UpdatePatientAsync(
                id: command.Id,
                applicationUserId: command.ApplicationUserId,
                title: command.Title,
                firstName: command.FirstName,
                middleName: command.MiddleName,
                lastName: command.LastName,
                phoneNumber: command.PhoneNumber,
                age: command.Age,
                cancellationToken: ct);

            return new UpdatePatientCommandResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified);
        }
    }
}
