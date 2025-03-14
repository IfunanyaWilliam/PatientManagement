
namespace PatientManagement.Application.Queries.Patient.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Infrastructure.Repositories.Interfaces;
    using Common.Handlers;
    using Common.Utilities;
    using Parameters;
    using Results;
    

    public class GetPatientQueryHandler : IQueryHandler<GetPatientQueryParameters, GetPatientQueryResult>
    {
        private readonly IPatientRepository _patientRepository;

        public GetPatientQueryHandler(IPatientRepository patientRepository) 
        {
            _patientRepository = patientRepository;
        }

        public async Task<GetPatientQueryResult> HandleAsync(
            GetPatientQueryParameters parameters, 
            CancellationToken ct = default)
        {
            if(parameters.PatientId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var patient = await _patientRepository.GetPatientAsync(parameters.PatientId);

            return new GetPatientQueryResult(
                id: patient.Id,
                applicationUserId: patient.ApplicationUserId,
                title: patient.Title,
                firstName: patient.FirstName,
                middleName: patient.MiddleName,
                lastName: patient.LastName,
                phoneNumber: patient.PhoneNumber,
                age: patient.Age,
                email: patient.Email,
                isActive: patient.IsActive,
                userRole: patient.UserRole,
                dateCreated: patient.DateCreated, 
                dateModified: patient.DateModified);
        }
    }
}
