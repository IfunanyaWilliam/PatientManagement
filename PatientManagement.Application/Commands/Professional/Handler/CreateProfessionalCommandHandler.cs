
namespace PatientManagement.Application.Commands.Professional.Handler
{
    using Microsoft.AspNetCore.Http;
    using Parameters;
    using Results;
    using Common.Handlers;
    using Infrastructure.Repositories.Interfaces;
    using Common.Utilities;

    public class CreateProfessionalCommandHandler :
        ICommandHandlerWithResult<CreateProfessionalCommandParameters, CreateProfessionalCommandResult>
    {

        private readonly IProfessionalRepository _professionalRepository;

        public CreateProfessionalCommandHandler(IProfessionalRepository professionalRepository) 
        {
            _professionalRepository = professionalRepository;
        }

        public async Task<CreateProfessionalCommandResult> HandleAsync(
            CreateProfessionalCommandParameters command, 
            CancellationToken ct = default)
        {
            if (command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _professionalRepository.CreateProfessionalAsync(
                applicationUserId: command.ApplicationUserId,
                title: command.Title,
                firstName: command.FirstName,
                middleName: command.MiddleName,
                lastName: command.LastName,
                phoneNumber: command.PhoneNumber,
                age: command.Age,
                qualification:  command.Qualification,
                license: command.License,
                userRole: command.UserRole,
                cancellationToken: ct);

            return new CreateProfessionalCommandResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                qualification: result.Qualification,
                license: result.License,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                createdDate: result.CreatedDate);
        }
    }
}
