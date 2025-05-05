
namespace PatientManagement.Application.Queries.Prescription.Handler
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.Threading;
    using Parameters;
    using Results;
    using Interfaces.Handlers;
    using Interfaces.Repositories;
    using Utilities;
    using Domain.Prescription;

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
                prescriptionId: prescription.PrescriptionId,
                diagnosis: prescription.Diagnosis,
                medications: prescription?.Medications?.Select(m =>
                    new PrescribedMedication(
                        medicationId: m.MedicationId,
                        name: m.Name,
                        dosage: m.Dosage,
                         instruction: m.Instruction,
                        isActive: m.IsActive)).ToList(),
                isActive: prescription.IsActive,
                dateCreated: prescription.DateCreated,
                dateModified: prescription.DateModified);
        }
    }
}
