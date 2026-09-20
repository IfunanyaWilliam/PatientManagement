

namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Application.Commands.Medication.Parameters;
    using Application.Commands.Medication.Results;
    using Application.Queries.Medication.Parameters;
    using Application.Queries.Medication.Results;
    using Infrastructure.PolicyProvider;
    using Application.Interfaces.Commands;
    using Application.Interfaces.Queries;
    using Parameters;
    using Results;
    using PatientManagement.Application.Utilities;

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class MedicationController : ControllerBase
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;
        private readonly IQueryExecutor _queryExecutor;

        public MedicationController(
            ICommandExecutorWithResult commandExecutorWithResult,
            IQueryExecutor queryExecutor)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
            _queryExecutor = queryExecutor;
        }


        /// <summary>
        ///     POST: /api/v1/medication/
        /// </summary>
        /// <remarks>
        ///     Create a medication
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
        [PermissionAuthorize(permission: "ManageMedicalRecords")]
        [ProducesResponseType(typeof(CreateMedicationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateMedication(
            [FromBody] CreateMedicationParameters parameters,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(parameters.Name) || string.IsNullOrWhiteSpace(parameters.Description))
                return BadRequest(
                    BaseResponse<CreateMedicationResult>.Fail(
                        error: "Medication Name and Description are required.",
                        message: "Input validation failed."));

            var result = await _commandExecutorWithResult
                .ExecuteAsync<CreateMedicationCommandParameters, CreateMedicationCommandResult>(
                    command: new CreateMedicationCommandParameters(
                        name: parameters.Name,
                        description: parameters.Description),
                    ct: ct);

            return StatusCode(
                StatusCodes.Status201Created,
                BaseResponse<CreateMedicationResult>.Success(
                    data: new CreateMedicationResult(
                            id: result.Id,
                            name: result.Name,
                            description: result.Description,
                            isActive: result.IsActive,
                            createdDate: result.CreatedDate,
                            dateModified: result.DateModified),
                    message: "Medication added successfully"));
        }

        /// <summary>
        ///     POST: /api/v1/medication/
        /// </summary>
        /// <remarks>
        ///     Update a medication
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
        [PermissionAuthorize(permission: "ManageMedicalRecords")]
        [ProducesResponseType(typeof(UpdateMedicationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMedication(
            [FromBody] UpdateMedicationParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters.MedicationId == Guid.Empty 
                || string.IsNullOrWhiteSpace(parameters.Name)
                || string.IsNullOrWhiteSpace(parameters.Description))
            {
                return BadRequest(
                    BaseResponse<CreatedAtActionResult>.Fail(
                        error: "One or more of your input parameters are empty.",
                        message: "Input validation failed."));
            }

            var result = await _commandExecutorWithResult
                .ExecuteAsync<UpdateMedicationCommandParameters, UpdateMedicationCommandResult>(
                    command: new UpdateMedicationCommandParameters(
                        medicationId: parameters.MedicationId,
                        name: parameters.Name,
                        description: parameters.Description),
                    ct: ct);

            return Ok(BaseResponse<UpdateMedicationResult>.Success(
                        data: new UpdateMedicationResult(
                                   id: result.Id,
                                   name: result.Name,
                                   description: result.Description,
                                   isActive: result.IsActive,
                                   createdDate: result.CreatedDate,
                                   dateModified: result.DateModified),
                        message: "Medication updated successfully."));
        }

        /// <summary>
        ///     GET: /api/v1/medication/
        /// </summary>
        /// <remarks>
        ///     Get a medication based on Id.
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
        [ProducesResponseType(typeof(GetMedicationByIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMedication(
            [FromRoute] Guid id,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(
                    BaseResponse<GetMedicationByIdResult>.Fail(
                        error: "Medication Id is required.",
                        message: "Input validation failed."));
            }

            var result = await _queryExecutor
                .ExecuteAsync<GetMedicationByIdQueryParameters, GetMedicationByIdQueryResult>(
                    parameters: new GetMedicationByIdQueryParameters(id: id),
                    ct: ct);

            return Ok(BaseResponse<GetMedicationByIdResult>.Success(
                data: new GetMedicationByIdResult(
                            id: result.Id,
                            name: result.Name,
                            isActive: result.IsActive,
                            description: result.Description,
                            createdDate: result.DateCreated,
                            dateModified: result.DateModified),
                message: "Medication details retrieved."));
        }


        /// <summary>
        ///     GET: /api/v1/medication/all
        /// </summary>
        /// <remarks>
        ///     Get all medications
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
        [ProducesResponseType(typeof(GetAllMedicationsResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMedications(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchParam = null,
            CancellationToken ct = default)
        {
            var result = await _queryExecutor
                .ExecuteAsync<GetAllMedicationsQueryParameters, GetAllMedicationsQueryResult>(
                    parameters: new GetAllMedicationsQueryParameters(
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                        searchParam: searchParam),
                    ct: ct);

            if (result.Medications is null || !result.Medications.Any())
            {
                return Ok(BaseResponse<GetAllMedicationsResult>.Success(
                    data: new GetAllMedicationsResult(new List<GetMedicationsResult>()),
                    message: "No medication found"));
            }

            return Ok(BaseResponse<GetAllMedicationsResult>.Success(
                    data: new GetAllMedicationsResult(
                            result.Medications.Select(m =>
                                  new GetMedicationsResult(
                                      id: m.Id,
                                      name: m.Name,
                                      description: m.Description,
                                      isActive: m.IsActive,
                                      dateCreated: m.DateCreated,
                                      dateModified: m.DateModified))),
                    message: "Medications retrieved successfully."));
        }
    }
}
