
namespace PatientManagement.Application.Queries.Medication.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Infrastructure.Repositories.Interfaces;
    using Common.Utilities;
    using Common.Handlers;
    using Parameters;
    using Results;
    
    
    public class GetMedicationByIdHandler : IQueryHandler<GetMedicationByIdQueryParameters, GetMedicationByIdQueryResult>
    {
        private readonly IMedicationRepository _medicationRepository;

        public GetMedicationByIdHandler(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<GetMedicationByIdQueryResult> HandleAsync(
            GetMedicationByIdQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters.Id == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var medication = await _medicationRepository.GetMedicationByIdAsync(id: parameters.Id, cancellationToken: ct);

            if (medication == null)
                    throw new CustomException($"Medication with Id: {parameters.Id} Not Found", StatusCodes.Status400BadRequest);

            return new GetMedicationByIdQueryResult(
                id: medication.Id,
                name: medication.Name,
                isActive: medication.IsActive,
                description: medication.Description,
                dateCreated: medication.DateCreated,
                dateModified: medication.DateModified);
        }
    }
}
