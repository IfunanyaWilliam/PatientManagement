
namespace PatientManagement.Application.Commands.Patient.Handler
{
    using Parameters;
    using Results;
    using Common.Handlers;
    using Infrastructure.Repositories.Interfaces;
    using Microsoft.AspNetCore.Http;
    using PatientManagement.Common.Utilities;

    public class CreatePatientCommandHandler :
        ICommandHandlerWithResult<CreatePatientCommandParameters, CreatePatientCommandResult>
    {

        private readonly IPatientRepository _patientRepository;

        public CreatePatientCommandHandler(
            IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository
                                    ?? throw new ArgumentNullException(nameof(patientRepository)); 
        }

        public async Task<CreatePatientCommandResult> HandleAsync(CreatePatientCommandParameters command, CancellationToken ct = default)
        {
            if(command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _patientRepository.CreatePatientAsync(
                applicationUserId: command.ApplicationUserId,
                title: command.Title,
                firstName: command.FirstName,
                middleName: command.MiddleName,
                lastName: command.LastName,
                phoneNumber: command.PhoneNumber,
                age: command.Age,
                cancellationToken: ct);

            return new CreatePatientCommandResult(
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
                createdDate: result.CreatedDate);
        }
    }
}
