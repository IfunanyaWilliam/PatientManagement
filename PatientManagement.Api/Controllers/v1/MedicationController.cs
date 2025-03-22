

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



    }
}
