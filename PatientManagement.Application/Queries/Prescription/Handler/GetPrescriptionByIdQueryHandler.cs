
namespace PatientManagement.Application.Queries.Prescription.Handler
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.Threading;
    using Parameters;
    using Results;
    using Common.Dto;
    using Common.Utilities;
    using Common.Handlers;
    using Infrastructure.Repositories.Interfaces;    

    public class GetPrescriptionByIdQueryHandler : IQueryHandler<GetPrescriptionByIdQueryParameters, GetPrescriptionByIdQueryResult>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        public GetPrescriptionByIdQueryHandler(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<GetPrescriptionByIdQueryResult> HandleAsync(
            GetPrescriptionByIdQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters.PrescriptionId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var prescription = await _prescriptionRepository.GetPrescriptionByIdAsync(parameters.PrescriptionId, ct);

            return new GetPrescriptionByIdQueryResult(
                id: prescription.Id,
                patientId: prescription.PatientId,
                professionalId: prescription.ProfessionalId,
                diagnosis: prescription.Diagnosis,
                medications: prescription?.Medications?.Select(m =>
                    new PrescribedMedication(
                        id: m.Id,
                        name: m.Name,
                        dosage: m.Dosage,
                        isActive: m.IsActive)).ToList(),
                isActive: prescription.IsActive,
                createdDate: prescription.CreatedDate,
                dateModified: prescription.DateModified);
        }
    }
}
