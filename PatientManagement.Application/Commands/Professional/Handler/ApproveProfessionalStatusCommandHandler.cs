
namespace PatientManagement.Application.Commands.Professional.Handler
{
    using Microsoft.AspNetCore.Http;
    using Parameters;
    using Results;
    using Interfaces.Repositories;
    using Utilities;
    using Interfaces.Handlers;

    public class ApproveProfessionalStatusCommandHandler :
        ICommandHandlerWithResult<ApproveProfessionalStatusCommandParameters, ApproveProfessionalStatusCommandResult>
    {

        private readonly IProfessionalRepository _professionalRepository;

        public ApproveProfessionalStatusCommandHandler(IProfessionalRepository professionalRepository)
        {
            _professionalRepository = professionalRepository;
        }

        public async Task<ApproveProfessionalStatusCommandResult> HandleAsync(
            ApproveProfessionalStatusCommandParameters command, 
            CancellationToken ct = default)
        {
            if (command == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _professionalRepository.ApproveProfessionalStatusAsync(
                command.ProfessionalId);

            return new ApproveProfessionalStatusCommandResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result?.FirstName,
                middleName: result?.MiddleName,
                lastName: result?.LastName,
                phoneNumber: result?.PhoneNumber,
                age: result.Age,
                qualification: result?.Qualification,
                license: result?.License,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                professionalStatus: result.ProfessionalStatus,
                dateCreated: result.DateCreated,
                dateModified: result?.DateModified);
        }
    }
}
