
namespace PatientManagement.Api.Controllers.v1
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Infrastructure.PolicyProvider;
    using Application.Commands.Prescription.Parameters;
    using Application.Commands.Prescription.Results;
    using Application.Queries.Prescription.Results;
    using Application.Queries.Prescription.Parameters;
    using Common.Dto;
    using Common.Enums;
    using Common.Results;
    using Common.Contracts;
    using Common.Parameters;
    

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly ICommandExecutorWithResult _commandExecutorWithResult;
        private readonly IQueryExecutor _queryExecutor;

        public PrescriptionController(
            ICommandExecutorWithResult commandExecutorWithResult,
            IQueryExecutor queryExecutor)
        {
            _commandExecutorWithResult = commandExecutorWithResult;
            _queryExecutor = queryExecutor;
        }

        [HttpPost]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(CreatePrescriptionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePrescriptionAsync(
            [FromBody] CreatePrescriptionParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<CreatePrescriptionCommandParameters, CreatePrescriptionCommandResult>(
                    command: new CreatePrescriptionCommandParameters(
                        patientId: parameters.PatientId,
                        professionalId: parameters.ProfessionalId,
                        diagnosis: parameters.Diagnosis,
                        medications: parameters.Medications?.Select(m => 
                            new MedicationParameters(
                                medicationId: m.MedicationId,
                                dosage: m.Dosage,
                                instruction: m.Instruction))),
                    ct: ct);

            return Ok(new CreatePrescriptionResult(
                id: result.Id,
                patientId: result.PatientId,
                professionalId: result.ProfessionalId,
                diagnosis: result.Diagnosis,
                isActive: result.IsActive,
                dateCreated: result.DateCreated));
        }

        [HttpPut]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(UpdatePrescriptionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePrescriptionAsync(
            [FromBody] UpdatePrescriptionParameters parameters,
            CancellationToken ct = default)
        {
            if (parameters == null)
                return BadRequest("Parameters must not be null.");

            var result = await _commandExecutorWithResult
                .ExecuteAsync<UpdatePrescriptionCommandParameters, UpdatePrescriptionCommandResult>(
                    command: new UpdatePrescriptionCommandParameters(
                        prescriptionId: parameters.PrescriptionId,
                        patientId: parameters.PatientId,
                        professionalId: parameters.ProfessionalId,
                        diagnosis: parameters.Diagnosis,
                        medications: parameters.Medications?.Select(m =>
                            new MedicationParameters(
                                medicationId: m.MedicationId,
                                dosage: m.Dosage,
                                instruction: m.Instruction))),
                    ct: ct);

            return Ok(new UpdatePrescriptionResult(
                id: result.Id,
                patientId: result.PatientId,
                professionalId: result.ProfessionalId,
                diagnosis: result.Diagnosis,
                isActive: result.IsActive,
                createdDate: result.CreatedDate,
                dateModified: result.DateModified));
        }


        [HttpGet("{id}")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetPrescriptionByIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPrescriptionByIdAsync(Guid id, 
            CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return BadRequest("PrescriptionId is required");
            
            var result = await _queryExecutor
                .ExecuteAsync<GetPrescriptionByIdQueryParameters, GetPrescriptionByIdQueryResult>(
                    parameters: new GetPrescriptionByIdQueryParameters(prescriptionId: id),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "Your request could not be processed now, try again later." });

            return Ok(new GetPrescriptionByIdResult(
                id: result.Id,
                patientId: result.PatientId,
                professionalId: result.ProfessionalId,
                prescriptionId: result.PrescriptionId,
                diagnosis: result.Diagnosis,
                medications: result?.Medications?.Select(m =>
                    new PrescribedMedication(
                        medicationId: m.MedicationId,
                        name: m.Name,
                        dosage: m.Dosage,
                        instruction: m.Instruction,
                        isActive: m.IsActive)).ToList(),
                isActive: result.IsActive,
                dateCreated: result.DateCreated,
                dateModified: result.DateModified));
        }

        [HttpGet("{patientId}")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ViewMedicalRecords", "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetPrescriptionByPatientIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPrescriptionByPatientIdAsync(
            Guid patientId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            if (patientId == Guid.Empty)
                return BadRequest("PrescriptionId is required");

            var result = await _queryExecutor
                .ExecuteAsync<GetPrescriptionByPatientIdQueryParameters, GetPrescriptionByPatientIdQueryResult>(
                    parameters: new GetPrescriptionByPatientIdQueryParameters(
                        patientId: patientId,
                        pageNumber: pageNumber,
                        pageSize: pageSize),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "Your request could not be processed now, try again later." });

            return Ok(new GetPrescriptionByPatientIdResult(
                prescriptions: result.Prescriptions.Select(p =>
                        new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p?.Medications?.Select(m =>
                                new PrescribedMedication(
                                    medicationId: m.MedicationId,
                                    name: m.Name,
                                    dosage: m.Dosage,
                                     instruction: m.Instruction,
                                    isActive: m.IsActive)).ToList(),
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified))));
        }

        [HttpGet("{professionalId}")]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetPrescriptionByProfessionalIdResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPrescriptionByProfessionalIdAsync(
            Guid professionalId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            if (professionalId == Guid.Empty)
                return BadRequest("PrescriptionId is required");

            var result = await _queryExecutor
                .ExecuteAsync<GetPrescriptionByProfessionalIdQueryParameters, GetPrescriptionByProfessionalIdQueryResult>(
                    parameters: new GetPrescriptionByProfessionalIdQueryParameters(
                        professionalId: professionalId,
                        pageNumber: pageNumber,
                        pageSize: pageSize),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "Your request could not be processed now, try again later." });

            return Ok(new GetPrescriptionByProfessionalIdResult(
                prescriptions: result.Prescriptions.Select(p =>
                        new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p?.Medications?.Select(m =>
                                new PrescribedMedication(
                                    medicationId: m.MedicationId,
                                    name: m.Name,
                                    dosage: m.Dosage,
                                     instruction: m.Instruction,
                                    isActive: m.IsActive)).ToList(),
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified))));
        }

        [HttpGet]
        [PermissionAuthorize(permissionOperator: PermissionOperator.Or, "ManagePatientRecords", "ManageMedicalRecords")]
        [ProducesResponseType(typeof(GetAllPrescriptionsResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPrescriptionsAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchParam = null,
            CancellationToken ct = default)
        {
            var result = await _queryExecutor
                .ExecuteAsync<GetAllPrescriptionsQueryParameters, GetAllPrescriptionsQueryResult>(
                    parameters: new GetAllPrescriptionsQueryParameters(
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                        searchParam: searchParam),
                    ct: ct);

            if (result == null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "Your request could not be processed now, try again later." });

            if (result.Prescriptions == null || !result.Prescriptions.Any())
                return Ok(new GetAllPrescriptionsResult(new List<Common.Dto.PrescriptionDto>()));

            return Ok(new GetAllPrescriptionsResult(
                   prescriptions: result.Prescriptions.Select(p => 
                        new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p?.Medications?.Select(m =>
                                new PrescribedMedication(
                                    medicationId: m.MedicationId,
                                    name: m.Name,
                                    dosage: m.Dosage,
                                    instruction: m.Instruction,
                                    isActive: m.IsActive)).ToList(),
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified))));
        }
    }
}
