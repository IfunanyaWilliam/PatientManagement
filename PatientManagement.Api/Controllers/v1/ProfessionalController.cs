
namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Application.Commands.Professional.Parameters;
    using Application.Commands.Professional.Results;
    using Infrastructure.PolicyProvider;
    using Common.Contracts;
    using Common.Parameters;
    using Common.Results;
    using Common.Enums;
    

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class ProfessionalController : Controller
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;
        private readonly IQueryExecutor _queryExecutor;

        public ProfessionalController(
            ICommandExecutorWithResult commandExecutorWithResult,
            IQueryExecutor queryExecutor)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
            _queryExecutor = queryExecutor;
        }


        [HttpPost]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ManageProfessionalRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(CreateProfessionalResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProfessionalAsync(
            [FromBody] CreateProfessionalParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameter values are required");
            
            var result = await _commandExecutorWithResult
                .ExecuteAsync<CreateProfessionalCommandParameters, CreateProfessionalCommandResult>(
                    command: new CreateProfessionalCommandParameters(
                        applicationUserId: parameters.ApplicationUserId,
                        title: parameters.Title,
                        firstName: parameters.FirstName,
                        middleName: parameters.MiddleName,
                        lastName: parameters.LastName,
                        phoneNumber: parameters.PhoneNumber,
                        age: parameters.Age,
                        qualification: parameters.Qualification,
                        license: parameters.License,
                        userRole: parameters.UserRole),
                    ct: ct);

            return Ok(new CreateProfessionalResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                qualification: result.Qualification,
                license: result.License,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                createdDate: result.CreatedDate));
        }

        [HttpPut]
        [PermissionAuthorize(permission: "ManageMedicalRecords")]
        [ProducesResponseType(typeof(ApproveProfessionalStatusResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ApproveProfessionalStatusAsync(
            [FromBody] ApproveProfessionalStatusParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<ApproveProfessionalStatusCommandParameters, ApproveProfessionalStatusCommandResult>(
                    command: new ApproveProfessionalStatusCommandParameters(
                        professionalId: parameters.ProfessionalId),
                    ct: ct);

            return Ok(new ApproveProfessionalStatusResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                qualification: result.Qualification,
                license: result.License,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                professionalStatus: result.ProfessionalStatus,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified));
        }
    }
}
