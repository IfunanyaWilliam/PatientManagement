

namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Application.Commands.Medication.Parameters;
    using Application.Commands.Medication.Results;
    using Infrastructure.PolicyProvider;
    using Common.Contracts;
    using Common.Parameters;
    using Common.Results;
    using Common.Enums;
    using PatientManagement.Application.Queries.Patient.Parameters;
    using PatientManagement.Application.Queries.Patient.Results;
    using PatientManagement.Application.Queries.Medication.Parameters;
    using PatientManagement.Application.Queries.Medication.Results;

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
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<CreateMedicationCommandParameters, CreateMedicationCommandResult>(
                    command: new CreateMedicationCommandParameters(
                        name: parameters.Name,
                        description: parameters.Description),
                    ct: ct);

            return Ok(new CreateMedicationResult(
                id: result.Id,
                name: result.Name,
                description: result.Description,
                isActive: result.IsActive,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified));
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
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<UpdateMedicationCommandParameters, UpdateMedicationCommandResult>(
                    command: new UpdateMedicationCommandParameters(
                        medicationId: parameters.MedicationId,
                        name: parameters.Name,
                        description: parameters.Description),
                    ct: ct);

            return Ok(new UpdateMedicationResult(
               id: result.Id,
               name: result.Name,
               description: result.Description,
               isActive: result.IsActive,
               createdDate: result.CreatedDate,
               dateModified: result.DateModified));
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
                return BadRequest("Patient Id is required");

            var result = await _queryExecutor
                .ExecuteAsync<GetMedicationByIdQueryParameters, GetMedicationByIdQueryResult>(
                    parameters: new GetMedicationByIdQueryParameters(id: id),
                    ct: ct);

            return Ok(new GetMedicationByIdResult(
                id: result.Id,
                name: result.Name,
                isActive: result.IsActive,
                description: result.Description,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified));
        }
    }
}
