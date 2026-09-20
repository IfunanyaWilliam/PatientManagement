
namespace PatientManagement.Api.Controllers.v1
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Application.Commands.Professional.Parameters;
    using Application.Commands.Professional.Results;
    using Application.Interfaces.Commands;
    using Application.Interfaces.Queries;
    using Application.Queries.Professional.Parameters;
    using Application.Queries.Professional.Resulsts;
    using Infrastructure.PolicyProvider;
    using Asp.Versioning;
    using FluentValidation;
    using Parameters;
    using Results;
    using PatientManagement.Application.Utilities;

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class ProfessionalController : Controller
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;
        private readonly IQueryExecutor _queryExecutor;
        private readonly IValidator<ApproveProfessionalStatusParameters> _approveValidator;
        private readonly IValidator<CreateProfessionalParameters> _createProfessionalValidator;


        public ProfessionalController(
            ICommandExecutorWithResult commandExecutorWithResult,
            IQueryExecutor queryExecutor,
            IValidator<ApproveProfessionalStatusParameters> approveValidator,
            IValidator<CreateProfessionalParameters> createProfessionalValidator)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
            _queryExecutor = queryExecutor;
            _approveValidator = approveValidator;
            _createProfessionalValidator = createProfessionalValidator;
        }


        /// <summary>
        ///     POST: /api/v1/professional/
        /// </summary>
        /// <remarks>
        ///     Create a professional
        /// </remarks>
        /// <param name="parameters"></param>
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
        [HttpPost]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ManageProfessionalRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(CreateProfessionalResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProfessionalAsync(
            [FromBody] CreateProfessionalParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest(BaseResponse<CreateProfessionalResult>.Fail(
                    error: "Parameter values are required",
                    message: "Invalid parameters"));

            var validationResult = await _createProfessionalValidator.ValidateAsync(parameters, ct);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

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

            return Ok(BaseResponse<CreateProfessionalResult>.Success(
                data: new CreateProfessionalResult(
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
                    dateCreated: result.DateCreated,
                    dateModified: result.DateModified),
                message: "Professional created successfully",
                responseCode: StatusCodes.Status200OK));
        }


        /// <summary>
        ///     PUT: /api/v1/professional/
        /// </summary>
        /// <remarks>
        ///     Create a professional
        /// </remarks>
        /// <param name="parameters"></param>
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
        [HttpPut]
        [PermissionAuthorize(permission:  "ManageMedicalRecords")]
        [ProducesResponseType(typeof(ApproveProfessionalStatusResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ApproveProfessionalStatusAsync(
            [FromBody] ApproveProfessionalStatusParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null) {

                return BadRequest(BaseResponse<CreateProfessionalResult>.Fail(
                    error: "Parameter values are required",
                    message: "No request body provided"));
            }

            var validationResult = await _approveValidator.ValidateAsync(parameters, ct);

            if (!validationResult.IsValid)
            {
                return BadRequest(BaseResponse<CreateProfessionalResult>.Fail(
                    errors: validationResult.Errors.Select(e => e.ErrorMessage),
                    message: "Validation failed"));
            }

            var result = await _commandExecutorWithResult
                .ExecuteAsync<ApproveProfessionalStatusCommandParameters, ApproveProfessionalStatusCommandResult>(
                    command: new ApproveProfessionalStatusCommandParameters(
                        professionalId: parameters.ProfessionalId),
                    ct: ct);

            return Ok(BaseResponse<ApproveProfessionalStatusResult>.Success(
                data: new ApproveProfessionalStatusResult(
                    id: result.Id,
                    applicationUserId: result.ApplicationUserId,
                    title: result.Title,
                    firstName: result?.FirstName,
                    middleName: result?.MiddleName,
                    lastName: result?.LastName,
                    phoneNumber: result?.PhoneNumber,
                    age: result.Age,
                    qualification: result?.Qualification,
                    license: result?.License,
                    email: result?.Email,
                    isActive: result.IsActive,
                    userRole: result.UserRole,
                    professionalStatus: result.ProfessionalStatus,
                    dateCreated: result.DateCreated,
                    dateModified: result?.DateModified),
                message: "Professional status approved successfully"));
        }

        /// <summary>
        ///     GET: /api/v1/professional/
        /// </summary>
        /// <remarks>
        ///     Gets a professional
        /// </remarks>
        /// <param name="professionalId"></param>
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
        [HttpGet("{professionalId}")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManageProfessionalRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetProfessionalByIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfessional(
            [FromRoute] Guid professionalId,
            CancellationToken ct = default)
        {
            if (professionalId == Guid.Empty)
                return BadRequest(BaseResponse<GetProfessionalByIdResult>.Fail(
                    error: "Professional Id is required",
                    message: "No professional Id provided"));

            var result = await _queryExecutor
            .ExecuteAsync<GetProfessionalByIdQueryParameters, GetProfessionalByIdQueryResult>(
            parameters: new GetProfessionalByIdQueryParameters(professionalId: professionalId),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    BaseResponse<GetProfessionalByIdResult>.Fail(
                        error: "Professional not found",
                        message: $"Professional with Id: {professionalId} Not Found.",
                        responseCode: StatusCodes.Status404NotFound));

            return Ok(BaseResponse<GetProfessionalByIdResult>.Success(
                data: new GetProfessionalByIdResult(
                    id: result.Id,
                    applicationUserId: result.ApplicationUserId,
                    title: result.Title,
                    firstName: result?.FirstName,
                    middleName: result?.MiddleName,
                    lastName: result?.LastName,
                    phoneNumber: result?.PhoneNumber,
                    age: result.Age,
                    qualification: result?.Qualification,
                    license: result?.License,
                    email: result?.Email,
                    isActive: result.IsActive,
                    userRole: result.UserRole,
                    professionalStatus: result.ProfessionalStatus,
                    dateCreated: result.DateCreated,
                    dateModified: result?.DateModified),
                message: "Professional retrieved successfully"));
        }


        /// <summary>
        ///     GET: /api/v1/professional/all
        /// </summary>
        /// <remarks>
        ///     Gets a professional
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
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManageProfessionalRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetAllProfessionalsResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllProfessionalsResult>> GetAllProfessionals(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchParam = null,
            CancellationToken ct = default)
        {
            var result = await _queryExecutor
                .ExecuteAsync<GetAllProfessionalsQueryParameters, GetAllProfessionalsQueryResult>(
                    parameters: new GetAllProfessionalsQueryParameters(
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                        searchParam: searchParam),
                    ct: ct);

            if (result == null)
                return Ok(BaseResponse<GetAllProfessionalsResult>.Success(
                    data: new GetAllProfessionalsResult(new List<GetProfessionalsResult>()),
                    message: "No professionals found"));

            return Ok(BaseResponse<GetAllProfessionalsResult>.Success(
                data: new GetAllProfessionalsResult(
                    professionals: result.Professionals.Select(p =>
                        new GetProfessionalsResult(
                            id: p.Id,
                            applicationUserId: p.ApplicationUserId,
                            title: p.Title,
                            firstName: p?.FirstName,
                            middleName: p?.MiddleName,
                            lastName: p?.LastName,
                            phoneNumber: p?.PhoneNumber,
                            age: p.Age,
                            qualification: p?.Qualification,
                            license: p?.License,
                            email: p?.Email,
                            isActive: p.IsActive,
                            userRole: p.UserRole,
                            professionalStatus: p.ProfessionalStatus,
                            dateCreated: p.DateCreated,
                            dateModified: p?.DateModified))),
                message: "Professionals retrieved successfully"));
        }
    }
}
