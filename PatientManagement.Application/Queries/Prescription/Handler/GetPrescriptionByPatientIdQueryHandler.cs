
namespace PatientManagement.Application.Queries.Prescription.Handler
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Results;
    using Parameters;
    using Interfaces.Repositories;
    using Interfaces.Handlers;
    using Utilities;
    using Domain.Prescription;

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

            var result = await _prescriptionRepository.GetPrescriptionsByPatientIdAsync(
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

                return new GetPrescriptionByPatientIdQueryResult(new List<GetPrescriptionQueryResult>());
            }

            return new GetPrescriptionByPatientIdQueryResult(
                prescriptions: result.Select(p => new GetPrescriptionQueryResult(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            prescriptionId: p.PrescriptionId,
                            symptoms: p.Symptoms,
                            diagnosis: p.Diagnosis,
                            medications: p.Medications?.Select(m => new PrescribedMedication(
                                medicationId: m.MedicationId,
                                name: m.Name,
                                dosage: m.Dosage,
                                 instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                            isActive: p.IsActive,
                            dateCreated: p.DateCreated,
                            dateModified: p.DateModified)));
        }
    }
}
