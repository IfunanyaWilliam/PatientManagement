
namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Application.Commands.Patient.Parameters;
    using Application.Commands.Patient.Results;
    using Application.Queries.Patient.Parameters;
    using Application.Queries.Patient.Results;
    using Infrastructure.PolicyProvider;
    using Common.Contracts;
    using Common.Results;
    using Common.Enums;
    

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;
        private readonly IQueryExecutor _queryExecutor;

        public PatientController(
            ICommandExecutorWithResult commandExecutorWithResult, 
            IQueryExecutor queryExecutor)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
            _queryExecutor = queryExecutor;
        }


        [HttpPost]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "CreatePatient", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(CreatePatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePatientAsync(
            [FromBody] CreatePatientCommandParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<CreatePatientCommandParameters, CreatePatientCommandResult>(
                    command: new CreatePatientCommandParameters(
                        applicationUserId: parameters.ApplicationUserId,
                        title: parameters.Title,
                        firstName: parameters.FirstName,
                        middleName: parameters.MiddleName,
                        lastName: parameters.LastName,
                        phoneNumber: parameters.PhoneNumber,
                        age: parameters.Age),
                    ct: ct);

            return Ok(new CreatePatientResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                dateCreated: result.DateCreated,
                dateModified: result.DateModified));
        }


        [HttpPut]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(UpdatePatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePatientAsync(
            UpdatePatientCommandParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<UpdatePatientCommandParameters, UpdatePatientCommandResult>(
                    command: new UpdatePatientCommandParameters(
                        id:  parameters.Id,
                        applicationUserId: parameters.ApplicationUserId,
                        title: parameters.Title,
                        firstName: parameters.FirstName,
                        middleName: parameters.MiddleName,
                        lastName: parameters.LastName,
                        phoneNumber: parameters.PhoneNumber,
                        age: parameters.Age),
                    ct: ct);

            return Ok(new UpdatePatientResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified));
        }

        [HttpGet("{id}")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetPatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatient(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Patient Id is required");

            var result = await _queryExecutor
                .ExecuteAsync<GetPatientQueryParameters, GetPatientQueryResult>(
                    parameters: new GetPatientQueryParameters(patientId: id),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { message = "Your request could not be processed now, try again later." });

            return Ok(new GetPatientResult(
                id: result.Id,
                applicationUserId: result.ApplicationUserId,
                title: result.Title,
                firstName: result.FirstName,
                middleName: result.MiddleName,
                lastName: result.LastName,
                phoneNumber: result.PhoneNumber,
                age: result.Age,
                email: result.Email,
                isActive: result.IsActive,
                userRole: result.UserRole,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified));
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize(permission: "DeleteMedicalRecords")]
        [ProducesResponseType(typeof(DeletePatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePatientAsync(
            Guid id,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Patient Id is required");
            var result = await _commandExecutorWithResult
                .ExecuteAsync<DeletePatientCommandParameters, DeletePatientCommandResult>(
                    command: new DeletePatientCommandParameters(id: id),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "Your request could not be processed at the moment, try again later." });

            return Ok(new DeletePatientResult(result.IsDeleted));
        }
    }
}
