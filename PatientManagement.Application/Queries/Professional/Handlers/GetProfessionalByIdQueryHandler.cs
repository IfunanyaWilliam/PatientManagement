
namespace PatientManagement.Application.Queries.Professional.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Infrastructure.Repositories.Interfaces;
    using Professional.Parameters;
    using Professional.Resulsts;
    using Common.Handlers;
    using Common.Utilities;

    public class GetProfessionalByIdQueryHandler : 
        IQueryHandler<GetProfessionalByIdQueryParameters, GetProfessionalByIdQueryResult>
    {
        private readonly IProfessionalRepository _professionalRepository;
        private readonly ILogger<GetProfessionalByIdQueryHandler> _logger;

        public GetProfessionalByIdQueryHandler(
           IProfessionalRepository professionalRepository,
            ILogger<GetProfessionalByIdQueryHandler> logger)
        {
            _professionalRepository = professionalRepository;
            _logger = logger;
        }

        public async Task<GetProfessionalByIdQueryResult> HandleAsync(
            GetProfessionalByIdQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var professional = await _professionalRepository.GetProfessionalByIdAsync(
                parameters.ProfessionalId, ct);

            if (professional == null)
                return null;

            return new GetProfessionalByIdQueryResult(
                id: professional.Id,
                applicationUserId: professional.ApplicationUserId,
                title: professional.Title,
                firstName: professional.FirstName,
                middleName: professional.MiddleName,
                lastName: professional.LastName,
                phoneNumber: professional.PhoneNumber,
                age: professional.Age,
                qualification: professional.Qualification,
                license: professional.License,
                email: professional.Email,
                isActive: professional.IsActive,
                userRole: professional.UserRole,
                professionalStatus: professional.ProfessionalStatus,
                dateCreated: professional.DateCreated,
                dateModified: professional.DateModified);
        }
    }
}
