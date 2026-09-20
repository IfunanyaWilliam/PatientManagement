
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
    using FluentValidation;
    using PatientManagement.Application.Utilities;

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;
        private readonly IQueryExecutor _queryExecutor;
        private readonly IValidator<CreatePatientParameters> _createPatientParametersValidator;
        private readonly IValidator<UpdatePatientParameters> _updatePatientParametersValidator;

        public PatientController(
            ICommandExecutorWithResult commandExecutorWithResult, 
            IQueryExecutor queryExecutor,
            IValidator<CreatePatientParameters> createPatiendParametersValidator,
            IValidator<UpdatePatientParameters> updatePatientParametersValidator)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
            _queryExecutor = queryExecutor;
            _createPatientParametersValidator = createPatiendParametersValidator;
            _updatePatientParametersValidator = updatePatientParametersValidator;
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
                return BadRequest(BaseResponse<CreatePatientResult>.Fail(
                    error: "Parameter values are required",
                    message: "🤷 No request body provided"));

            var validationResult = await _createPatientParametersValidator.ValidateAsync(parameters, ct);

            if (!validationResult.IsValid)
            {
                return BadRequest(BaseResponse<CreatePatientResult>.Fail(
                    errors: validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    message: "Input validation failed."));
            }

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

            return Ok(BaseResponse<CreatePatientResult>.Success(
                        data:new CreatePatientResult(
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
                                    dateModified: result.DateModified),
                        message: "✨ Patient created successfully.",
                        responseCode: StatusCodes.Status201Created));
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
                return BadRequest(BaseResponse<CreatePatientResult>.Fail(
                    error: "Parameter values are required",
                    message: "No request body provided"));

            var validationResult = await _updatePatientParametersValidator.ValidateAsync(parameters, ct);

            if (!validationResult.IsValid)
            {
                return BadRequest(BaseResponse<CreatePatientResult>.Fail(
                    errors: validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    message: "Validation failed"));
            }

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

            return Ok(BaseResponse<UpdatePatientResult>.Success(
                data: new UpdatePatientResult(
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
                    dateModified: result.DateModified),
                message: "✨ Patient updated successfully",
                responseCode: StatusCodes.Status200OK));
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
                return BadRequest(BaseResponse<GetPatientResult>.Fail(
                    error: "Patient Id is required",
                    message: "Invalid parameters"));

            var result = await _queryExecutor
                .ExecuteAsync<GetPatientQueryParameters, GetPatientQueryResult>(
                    parameters: new GetPatientQueryParameters(patientId: id),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    BaseResponse<GetPatientResult>.Fail(
                        error: "Your request could not be processed now, try again later.",
                        message: "Internal Server Error"));

            return Ok(BaseResponse<GetPatientResult>.Success(
                data: new GetPatientResult(
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
                    dateModified: result.DateModified),
                message: "✨ Patient retrieved successfully",
                responseCode: StatusCodes.Status200OK));
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
        public async Task<IActionResult> GetAllPatients(
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

            if (result is null || !result.Patients.Any())
                return Ok(BaseResponse<GetAllPatientsResult>.Success(
                    data: new GetAllPatientsResult(new List<GetPatientsResult>()),
                    message: "🤔 No patients found",
                    responseCode: StatusCodes.Status404NotFound));

            return Ok(BaseResponse<GetAllPatientsResult>.Success(
                data: new GetAllPatientsResult(
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
                        dateModified: p.DateModified))),
                message: "✨ Patients retrieved successfully",
                responseCode: StatusCodes.Status200OK));
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
                return BadRequest(BaseResponse<DeletePatientResult>.Fail(
                    error: "Patient Id is required",
                    message: "🤔 No Patient Id provided",
                    responseCode: StatusCodes.Status400BadRequest));

            var result = await _commandExecutorWithResult
                .ExecuteAsync<DeletePatientCommandParameters, DeletePatientCommandResult>(
                    command: new DeletePatientCommandParameters(id: id),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    BaseResponse<DeletePatientResult>.Fail(
                        error: "Opps! 🫣 Internal Server Error",
                        message: "Your request could not be processed at the moment, try again later.",
                        responseCode: StatusCodes.Status500InternalServerError));

            return Ok(BaseResponse<DeletePatientResult>.Success(
                data: new DeletePatientResult(result.IsDeleted),
                message: "Patient deleted successfully",
                responseCode: StatusCodes.Status200OK));
        }
    }
}
