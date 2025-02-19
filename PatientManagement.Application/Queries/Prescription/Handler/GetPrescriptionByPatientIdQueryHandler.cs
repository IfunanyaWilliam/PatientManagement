
namespace PatientManagement.Application.Queries.Prescription.Handler
{
    using PatientManagement.Common.Utilities;
    using Microsoft.AspNetCore.Http;
    using Parameters;
    using Results;
    using Common.Handlers;
    using Common.Dto;
    using Infrastructure.Repositories.Interfaces;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;

    public class GetPrescriptionByPatientIdQueryHandler 
        : IQueryHandler<GetPrescriptionByPatientIdQueryParameters, GetPrescriptionByPatientIdQueryResult>
    {
        private readonly ILogger<GetPrescriptionByPatientIdQueryHandler> _logger;
        private readonly IPrescriptionRepository _prescriptionRepository;

        public GetPrescriptionByPatientIdQueryHandler(
            IPrescriptionRepository prescriptionRepository,
            ILogger<GetPrescriptionByPatientIdQueryHandler> logger)
        {
            _prescriptionRepository = prescriptionRepository;
            _logger = logger;
        }

        public async Task<GetPrescriptionByPatientIdQueryResult> HandleAsync(
            GetPrescriptionByPatientIdQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters.PatientId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _prescriptionRepository.GetPrescriptionByPatientIdAsync(
                parameters.PatientId,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize,
                ct);

            if (result == null)
            {
                _logger.LogError("Internal Server Error {@Param}, {@Error}, {@DateTimeUtc}",
                    $"Body: {JsonSerializer.Serialize(parameters)}",
                    "GetPrescriptionByPatientIdQueryHandler: Prescriptions not found",
                    DateTime.UtcNow.AddHours(1));

                return new GetPrescriptionByPatientIdQueryResult(new List<PrescriptionDto>());
            }

            return new GetPrescriptionByPatientIdQueryResult(
                prescriptions: result.Prescriptions.Select(p => new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p.Medications?.Select(m => new PrescribedMedication(
                                id: m.Id,
                                name: m.Name,
                                dosage: m.Dosage,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified)).ToList());
        }
    }
}
