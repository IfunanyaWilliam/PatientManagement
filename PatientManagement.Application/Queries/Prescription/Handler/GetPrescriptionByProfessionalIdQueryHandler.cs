
namespace PatientManagement.Application.Queries.Prescription.Handler
{
    using Microsoft.AspNetCore.Http;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Parameters;
    using Results;
    using Interfaces.Handlers;
    using Interfaces.Repositories;
    using Utilities;
    using Domain.Prescription;

    public class GetPrescriptionByProfessionalIdQueryHandler : 
        IQueryHandler<GetPrescriptionByProfessionalIdQueryParameters, GetPrescriptionByProfessionalIdQueryResult>
    {

        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly ILogger<GetPrescriptionByProfessionalIdQueryHandler> _logger;

        public GetPrescriptionByProfessionalIdQueryHandler(
            IPrescriptionRepository prescriptionRepository,
            ILogger<GetPrescriptionByProfessionalIdQueryHandler> logger)
        {
            _prescriptionRepository = prescriptionRepository;
            _logger = logger;
        }

        public async Task<GetPrescriptionByProfessionalIdQueryResult> HandleAsync(
            GetPrescriptionByProfessionalIdQueryParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters.ProfessionalId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _prescriptionRepository.GetPrescriptionByProfessionalIdAsync(
                professionalId: parameters.ProfessionalId,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize,
                ct);

            if (result == null)
            {
                _logger.LogError("Internal Server Error {@Param}, {@Error}, {@DateTimeUtc}",
                    $"Body: {JsonSerializer.Serialize(parameters)}",
                    "GetPrescriptionByProfessionalIdQueryHandler: Prescriptions not found",
                    DateTime.UtcNow.AddHours(1));

                return new GetPrescriptionByProfessionalIdQueryResult(new List<GetPrescriptionQueryResult>());
            }

            return new GetPrescriptionByProfessionalIdQueryResult(
                        result.Select(p => new GetPrescriptionQueryResult(
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
                            dateModified: p.DateModified)).ToList());
        }
    }
}
