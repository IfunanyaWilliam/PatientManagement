
namespace PatientManagement.Application.Queries.Professional.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Resulsts;
    using Parameters;
    using System.Text.Json;
    using Interfaces.Repositories;
    using Utilities;
    using Interfaces.Handlers;

    public class GetAllProfessionalsQueryHandler :
        IQueryHandler<GetAllProfessionalsQueryParameters, GetAllProfessionalsQueryResult>
    {
        private readonly IProfessionalRepository _professionalRepository;
        private readonly ILogger<GetAllProfessionalsQueryHandler> _logger;

        public GetAllProfessionalsQueryHandler(
            IProfessionalRepository professionalRepository,
            ILogger<GetAllProfessionalsQueryHandler> logger)
        {
            _professionalRepository = professionalRepository;
            _logger = logger;
        }

        public async Task<GetAllProfessionalsQueryResult> HandleAsync(
            GetAllProfessionalsQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _professionalRepository.GetAllProfessionalsAsync(
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize,
                searchParam: parameters.SearchParam,
                cancellationToken: ct);

            if (result == null)
            {
                _logger.LogError("Internal Server Error {@Param}, {@Error}, {@DateTimeUtc}",
                    $"Body: {JsonSerializer.Serialize(parameters)}",
                    "GetAllProfessionalsQueryHandler: Professional not found",
                    DateTime.UtcNow.AddHours(1));

                return new GetAllProfessionalsQueryResult(new List<ProfessionalResult>());
            }

            return new GetAllProfessionalsQueryResult(
                        result.Select(p => 
                            new ProfessionalResult(
                                id: p.Id,
                                applicationUserId: p.ApplicationUserId,
                                title: p.Title,
                                firstName: p?.FirstName,
                                middleName: p?.MiddleName,
                                lastName: p?.LastName,
                                phoneNumber: p?.PhoneNumber,
                                age: p.Age,
                                qualification: p.Qualification,
                                license: p.License,
                                email: p?.Email,
                                isActive: p.IsActive,
                                userRole: p.UserRole,
                                professionalStatus: p.ProfessionalStatus,
                                dateCreated: p.DateCreated,
                                dateModified: p?.DateModified)));
        }
    }
}
