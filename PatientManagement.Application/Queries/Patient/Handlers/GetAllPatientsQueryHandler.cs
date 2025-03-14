
namespace PatientManagement.Application.Queries.Patient.Handlers
{
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using System.Text.Json;
    using Parameters;
    using Results;
    using Common.Handlers;
    using Common.Utilities;
    using Infrastructure.Repositories.Interfaces;
    using Common.Results;
    

    public class GetAllPatientsQueryHandler : IQueryHandler<GetAllPatientsQueryParameters, GetAllPatientsQueryResult>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<GetAllPatientsQueryHandler> _logger;

        public GetAllPatientsQueryHandler(
            IPatientRepository patientRepository,
            ILogger<GetAllPatientsQueryHandler> logger) 
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<GetAllPatientsQueryResult> HandleAsync(
            GetAllPatientsQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _patientRepository.GetAllPatientsAsync(
                        pageNumber: parameters.PageNumber,
                        pageSize: parameters.PageSize,
                        searchParam: parameters.SearchParam,
                        cancellationToken: ct);

            if (result == null || !result.Any())
            {
                _logger.LogError("Internal Server Error {@Param}, {@Error}, {@DateTimeUtc}",
                    $"Body: {JsonSerializer.Serialize(parameters)}",
                    "GetAllPatientsQueryHandler: Patients not found",
                    DateTime.UtcNow.AddHours(1));

                return new GetAllPatientsQueryResult(new List<GetPatientResult>());
            }

            return new GetAllPatientsQueryResult(
                        result.Select(p => new GetPatientResult(
                            id: p.Id,
                            applicationUserId: p.ApplicationUserId,
                            title: p.Title,
                            firstName: p.FirstName,
                            middleName: p.MiddleName,
                            lastName: p.LastName,
                            phoneNumber: p.PhoneNumber,
                            age: p.Age,
                            email: p.Email,
                            isActive: p.IsActive,
                            userRole: p.UserRole,
                            dateCreated: p.DateCreated,
                            dateModified: p.DateModified)));
        }
    }
}
