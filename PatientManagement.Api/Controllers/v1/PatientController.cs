
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
    using Application.Interfaces.Commands;
    using Application.Interfaces.Queries;
    using Parameters;
    using Results;

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


        /// <summary>
        ///     POST: /api/v1/patient
        /// </summary>
        /// <remarks>
        ///     Add a patient.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500" >
        ///     Internal Server Error.
        /// </response>
        /// <response code = "401" >
        ///     Unauthorized.
        /// </response>
        /// <response code = "403" >
        ///     Forbidden.
        /// </response>
        [HttpPost]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "CreatePatient", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(CreatePatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePatientAsync(
            [FromBody] CreatePatientParameters parameters,
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


        /// <summary>
        ///     PUT: /api/v1/patient
        /// </summary>
        /// <remarks>
        ///     Update a patient.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500" >
        ///     Internal Server Error.
        /// </response>
        /// <response code = "401" >
        ///     Unauthorized.
        /// </response>
        /// <response code = "403" >
        ///     Forbidden.
        /// </response>
        [HttpPut]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(UpdatePatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePatientAsync(
            UpdatePatientParameters parameters,
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


        /// <summary>
        ///     GET: /api/v1/patient/id
        /// </summary>
        /// <remarks>
        ///     Get a patient based on Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500" >
        ///     Internal Server Error.
        /// </response>
        /// <response code = "401" >
        ///     Unauthorized.
        /// </response>
        /// <response code = "403" >
        ///     Forbidden.
        /// </response>
        [HttpGet("{id}")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetPatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatient(
            [FromRoute] Guid id, 
            CancellationToken ct = default)
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
                dateCreated: result.CreatedDate,
                dateModified: result.DateModified));
        }


        /// <summary>
        ///     GET: /api/v1/patient/all
        /// </summary>
        /// <remarks>
        ///     Get all patients
        /// </remarks>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchParam"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500">
        ///     Internal Server Error.
        /// </response>
        /// /// <response code = "401" >
        ///     Unauthorized.
        /// </response>
        /// <response code = "403" >
        ///     Forbidden.
        /// </response>
        [HttpGet("all")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetAllPatientsResult), StatusCodes.Status200OK)]
        public async Task<GetAllPatientsResult> GetAllPatients(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchParam = null,
            CancellationToken ct = default)
        {
            var result = await _queryExecutor
                .ExecuteAsync<GetAllPatientsQueryParameters, GetAllPatientsQueryResult>(
                    parameters: new GetAllPatientsQueryParameters(
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                        searchParam: searchParam),
                    ct: ct);

            if (result is null)
                return new GetAllPatientsResult(new List<GetPatientsResult>());

            return new GetAllPatientsResult(
                    result.Patients.Select(p => new GetPatientsResult(
                        id: p.Id,
                        applicationUserId: p.ApplicationUserId,
                        title: p.Title,
                        firstName: p.FirstName,
                        middleName: p.MiddleName,
                        lastName: p.LastName,
                        phoneNumber: p.PhoneNumber,
                        age: p.Age,
                        email: p.Email,
                        isActive: p.IsActive,
                        userRole: p.UserRole,
                        dateCreated: p.DateCreated,
                        dateModified: p.DateModified)));
        }


        /// <summary>
        ///     GET: /api/v1/patient/id
        /// </summary>
        /// <remarks>
        ///     Delete a patients using patient Id
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <response code="200">
        ///     Operation was successful.
        /// </response>
        /// <response code="400">
        ///     Bad Request.
        /// </response>
        /// <response code = "500">
        ///     Internal Server Error.
        /// </response>
        /// <response code = "401" >
        ///     Unauthorized.
        /// </response>
        /// <response code = "403" >
        ///     Forbidden.
        /// </response>
        [HttpDelete("{id}")]
        [PermissionAuthorize(permission: "DeleteMedicalRecords")]
        [ProducesResponseType(typeof(DeletePatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePatientAsync(
            [FromRoute] Guid id,
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
