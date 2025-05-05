
namespace PatientManagement.Application.Queries.Prescription.Handler
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using Results;
    using Parameters;
    using Utilities;
    using Interfaces.Repositories;
    using Domain.Prescription;
    using Interfaces.Handlers;

    public class GetAllPrescriptionsQueryHandler 
        : IQueryHandler<GetAllPrescriptionsQueryParameters, GetAllPrescriptionsQueryResult>
    {

        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly ILogger<GetAllPrescriptionsQueryHandler> _logger;

        public GetAllPrescriptionsQueryHandler(
            IPrescriptionRepository prescriptionRepository,
            ILogger<GetAllPrescriptionsQueryHandler> logger)
        {
            _prescriptionRepository = prescriptionRepository;
            _logger = logger;
        }

        public async Task<GetAllPrescriptionsQueryResult> HandleAsync(
            GetAllPrescriptionsQueryParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _prescriptionRepository.GetAllPrescriptionsAsync(
                        pageNumber: parameters.PageNumber,
                        pageSize: parameters.PageSize,
                        searchParam: parameters.SearchParam,
                        cancellationToken: ct);

            if (result == null)
            {
                _logger.LogError("Internal Server Error {@Param}, {@Error}, {@DateTimeUtc}",
                    $"Body: {JsonSerializer.Serialize(parameters)}",
                    "GetAllPrescriptionsQueryHandler: Prescriptions not found",
                    DateTime.UtcNow.AddHours(1));

                return new GetAllPrescriptionsQueryResult(new List<GetPrescriptionQueryResult>());
            }

            return new GetAllPrescriptionsQueryResult(
                          prescriptions: result.Select(p => new GetPrescriptionQueryResult(
                                id: p.Id,
                                patientId: p.PatientId,
                                professionalId: p.ProfessionalId,
                                prescriptionId: p.PrescriptionId,
                                symptoms: p.Symptoms,
                                diagnosis: p.Diagnosis,
                                medications: p.Medications.Select(m => new PrescribedMedication(
                                    medicationId: m.MedicationId,
                                    name: m.Name,
                                    dosage: m.Dosage,
                                     instruction: m.Instruction,
                                    isActive: m.IsActive)).ToList(),
                                isActive: p.IsActive,
                                dateCreated: p.DateCreated,
                                dateModified: p.DateModified)));
        }
    }
}
