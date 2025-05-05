
namespace PatientManagement.Application.Queries.Medication.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Parameters;
    using Results;
    using Interfaces.Repositories;
    using Utilities;
    using Interfaces.Handlers;

    public class GetAllMedicationsQueryHandler : IQueryHandler<GetAllMedicationsQueryParameters, GetAllMedicationsQueryResult>
    {
        private readonly IMedicationRepository _medicationRepository;

        public GetAllMedicationsQueryHandler(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
        }

        public async Task<GetAllMedicationsQueryResult> HandleAsync(
            GetAllMedicationsQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if (parameters == null)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var result = await _medicationRepository.GetAllMedicationsAsync(
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize,
                searchParam: parameters.SearchParam,
                cancellationToken: ct);

            if(!result.Any() || result is null)
            {
                return new GetAllMedicationsQueryResult(new List<GetMedicationsQueryResult>());
            }

            return new GetAllMedicationsQueryResult(
                    result.Select(m => 
                        new GetMedicationsQueryResult(
                            id: m.Id,
                            name: m.Name,
                            description: m.Description,
                            isActive: m.IsActive,
                            dateCreated: m.DateCreated,
                            dateModified: m.DateModified)));
        }
    }
}
