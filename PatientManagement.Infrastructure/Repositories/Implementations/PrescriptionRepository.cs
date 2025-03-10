﻿
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using Common.Results;
    using DbContexts;
    using Common.Utilities;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;
    using Interfaces;
    using PatientManagement.Common.Dto;
    using PatientManagement.Infrastructure.Entities;
    using System.Linq.Expressions;
    using PatientManagement.Domain.Prescription;
    using System.Linq;
    using PatientManagement.Domain.Patient;
    using PatientManagement.Domain.Professional;

    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Entities.ApplicationUser> _userManager;
        private readonly ILogger<PrescriptionRepository> _logger;


        public PrescriptionRepository(
            AppDbContext context,
            UserManager<Entities.ApplicationUser> userManager,
            ILogger<PrescriptionRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<CreatePrescriptionResult> CreatePrescriptionAsync(
            Guid patientId,
            Guid professionalId,
            string diagnosis,
            List<Common.Dto.PrescriptionMedicationDto> medications,
            CancellationToken cancellationToken)
        {
            if (patientId == Guid.Empty || professionalId == Guid.Empty
                || string.IsNullOrEmpty(diagnosis) || medications.Count <= 0)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var patientEntity = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId, cancellationToken);

            if (patientEntity is null || patientEntity.IsDeleted)
                throw new CustomException($"Patient with Id {patientId} Not Found", StatusCodes.Status404NotFound);
            
            if (!patientEntity.IsActive)
                throw new CustomException($"Patient with Id {patientId} is currently in active," +
                    $"contact admin for support", StatusCodes.Status403Forbidden);

            var professionalEntity = await _context.Professionals.FirstOrDefaultAsync(p => p.Id == professionalId, cancellationToken);

            if (professionalEntity is null || professionalEntity.IsDeleted)
                throw new CustomException($"Professional with Id {professionalId} Not Found", StatusCodes.Status404NotFound);

            if (professionalEntity.ProfessionalStatus != Common.Enums.ProfessionalStatus.Active
                || !professionalEntity.IsActive)
            {
                throw new CustomException($"The Professional is not allowed to consult with patient at the moment, " +
                    $"Consult Admin for guide", StatusCodes.Status403Forbidden);
            }

            var exisitingPrescription = await _context.Prescriptions.FirstOrDefaultAsync(
                                p => p.Diagnosis == diagnosis &&
                                p.ProfessionalId == professionalId &&
                                p.PatientId == patientId &&
                                p.IsActive, 
                                cancellationToken);

            if(exisitingPrescription is not null && exisitingPrescription.IsActive)
            {
                throw new CustomException($"Cannot create duplicate prescription: An active prescription (ID: {exisitingPrescription.Id}) " +
                    $"already exists with diagnosis \"{diagnosis}\"", StatusCodes.Status409Conflict);
            }

            var prescription = new Entities.Prescription
            {
                PatientId = patientId,
                ProfessionalId = professionalId,
                Diagnosis = diagnosis,
                IsActive = true
            };

            await _context.AddAsync(prescription);

            var ListOfPrescriptionMedications = new List<PrescriptionMedication>();
            foreach (var medication in medications)
            {
                var existingMedication = await _context.Medications.FirstOrDefaultAsync(
                    m => m.Id == medication.MedicationId
                    && m.IsActive,
                    cancellationToken);

                if (existingMedication is null || !existingMedication.IsActive)
                {
                    throw new CustomException($"Medication with Id: {medication.MedicationId} Not Found",
                        StatusCodes.Status400BadRequest);
                }

                var medicationEntity = new PrescriptionMedication
                {
                    PatientId = patientId,
                    MedicationId =  existingMedication.Id,
                    PrescriptionId = prescription.Id,
                    ProfessionalId = professionalId,
                    Name = existingMedication.Name,
                    Dosage = medication.Dosage,
                    Instruction = medication.Instruction,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                };
                ListOfPrescriptionMedications.Add(medicationEntity);
            }

            await _context.PrescriptionMedications.AddRangeAsync(ListOfPrescriptionMedications);
            prescription.PrescriptionMedications = ListOfPrescriptionMedications;
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return new CreatePrescriptionResult(
                    id: prescription.Id,
                    patientId: prescription.PatientId,
                    professionalId: prescription.ProfessionalId,
                    diagnosis: prescription.Diagnosis,
                    medications: ListOfPrescriptionMedications.Select(m => new PrescribedMedication(
                        medicationId:m.Id,
                        name: m.Name,
                        dosage: m.Dosage,
                        instruction: m.Instruction,
                        isActive: m.IsActive)).ToList(),
                    isActive: prescription.IsActive,
                    createdDate: prescription.CreatedDate);
            }

            _logger.LogError($"Prescription could not be saved to db, data => {JsonSerializer.Serialize(prescription)}");
            throw new CustomException("Prescription could not be created, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<UpdatePrescriptionResult> UpdatePrescriptionAsync(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string diagnosis,
            string reasonForUpdate,
            List<PrescribedMedication> medications,
            CancellationToken cancellationToken)
        {
            if (patientId == Guid.Empty || professionalId == Guid.Empty
                || string.IsNullOrEmpty(diagnosis) || medications.Count <= 0)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var patientEntity = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId, cancellationToken);

            if (patientEntity is null || patientEntity.IsDeleted)
                throw new CustomException($"Patient with Id {patientId} Not Found", StatusCodes.Status404NotFound);

            if (!patientEntity.IsActive)
                throw new CustomException($"Patient with Id {patientId} is currently in active," +
                    $"contact admin for support", StatusCodes.Status403Forbidden);

            var professionalEntity = await _context.Professionals.FirstOrDefaultAsync(p => p.Id == professionalId, cancellationToken);

            if (professionalEntity is null || professionalEntity.IsDeleted)
                throw new CustomException($"Professional with Id {professionalId} Not Found", StatusCodes.Status404NotFound);

            if (professionalEntity.ProfessionalStatus != Common.Enums.ProfessionalStatus.Active
                || !professionalEntity.IsActive)
            {
                throw new CustomException($"The Professional is not allowed to consult with patient at the moment, " +
                    $"Consult Admin for guide", StatusCodes.Status403Forbidden);
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionMedications)
                .FirstOrDefaultAsync(p => 
                    p.Id == prescriptionId &&
                    p.PatientId == patientId && 
                    p.ProfessionalId == professionalId, 
                    cancellationToken);

            if (prescription is null)
                throw new CustomException($"Prescription with Id {prescriptionId} Not Found", StatusCodes.Status404NotFound);

            var existingPrescriptionMedications = prescription.PrescriptionMedications;
            var newPrescriptionMedications = new List<PrescriptionMedication>();
            foreach (var medication in medications)
            {
                if (!prescription.PrescriptionMedications.Any(m => m.MedicationId == medication.MedicationId))
                {
                    PrescriptionMedication newPrescriptionMedication = new PrescriptionMedication
                    {
                        PatientId = patientId,
                        MedicationId = medication.MedicationId,
                        PrescriptionId = prescription.Id,
                        ProfessionalId = professionalId,
                        Name = medication.Name,
                        Dosage = medication.Dosage,
                        Instruction = medication.Instruction,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(1),
                    };

                    newPrescriptionMedications.Add(newPrescriptionMedication);
                }
            };

            if(newPrescriptionMedications.Any())
            {
                await _context.PrescriptionMedications.AddRangeAsync(newPrescriptionMedications);
            }

            if(existingPrescriptionMedications is not null)
            {
                foreach (var item in prescription.PrescriptionMedications)
                {
                    var prescribedMedication = medications.FirstOrDefault(m => m.MedicationId == item.MedicationId);
                    item.Dosage = prescribedMedication.Dosage ?? item.Dosage;
                    item.Instruction = prescribedMedication.Instruction ?? item.Instruction;
                    item.IsActive = prescribedMedication.IsActive;
                    item.DateModified = DateTime.UtcNow.AddHours(1);
                }
            }

            if(newPrescriptionMedications.Count > 0)
                _context.PrescriptionMedications.UpdateRange(newPrescriptionMedications);

            var allMedications = newPrescriptionMedications.Concat(existingPrescriptionMedications).ToList();

            prescription.Diagnosis = diagnosis ?? prescription.Diagnosis;
            prescription.DateModified = DateTime.UtcNow.AddHours(1);
            prescription.PrescriptionMedications = allMedications;
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new UpdatePrescriptionResult(
                    id: prescription.Id,
                    patientId: prescription.PatientId,
                    professionalId: prescription.ProfessionalId,
                    diagnosis: prescription.Diagnosis,
                    medications: allMedications.Select(m => new PrescribedMedication(
                        medicationId: m.Id,
                        name: m.Name,
                        dosage: m.Dosage,
                        instruction: m.Instruction,
                        isActive: m.IsActive)).ToList(),
                    isActive: prescription.IsActive,
                    createdDate: prescription.CreatedDate,
                    dateModified: prescription.DateModified);
            }

            _logger.LogError($"Medications could not be updated to db, data => {JsonSerializer.Serialize(prescription)}");
            throw new CustomException("Prescription could not be Updated, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<GetPrescriptionByIdResult> GetPrescriptionByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var prescription = await _context.Prescriptions
                                            .Include(p => p.PrescriptionMedications.Where(m => m.IsActive))
                                            .AsSplitQuery()
                                            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);

            if (prescription is null)
            {
                _logger.LogError("Not Found {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {id}",
                    "Prescription not found",
                    DateTime.UtcNow.AddHours(1));

                throw new CustomException("Prescription not found", StatusCodes.Status404NotFound);
            }

            return new GetPrescriptionByIdResult(
                id: prescription.Id,
                patientId: prescription.PatientId,
                professionalId: prescription.ProfessionalId,
                diagnosis: prescription.Diagnosis,
                medications: prescription?.PrescriptionMedications?.Select(m =>
                    new PrescribedMedication(
                        medicationId: m.Id,
                        name: m.Name,
                        dosage: m.Dosage,
                        instruction: m.Instruction,
                        isActive: m.IsActive)).ToList(),
                isActive: prescription.IsActive,
                createdDate: prescription.CreatedDate,
                dateModified: prescription.DateModified);
        }

        public async Task<GetPrescriptionByPatientIdResult> GetPrescriptionByPatientIdAsync(
            Guid patientId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (patientId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            Expression<Func<Entities.Prescription, bool>> predicate = s => s.IsActive;

            predicate = s => s.IsActive && s.PatientId == patientId;

            var prescriptions = await _context.Prescriptions
                                        .Include(z => z.PrescriptionMedications)
                                        .Where(predicate)
                                        .OrderBy(x => x.CreatedDate)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if (prescriptions is null)
            {
                _logger.LogError("Not Found {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {patientId}",
                    "Prescription not found",
                    DateTime.UtcNow.AddHours(1));

                return null;
            }

            return new GetPrescriptionByPatientIdResult(
                prescriptions: prescriptions.Select(p => new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.Name,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified)).ToList());
        }

        public async Task<GetPrescriptionByProfessionalIdResult> GetPrescriptionByProfessionalIdAsync(
            Guid professionalId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            if (professionalId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            Expression<Func<Entities.Prescription, bool>> predicate = s => s.IsActive;
            
            predicate = s => s.IsActive && s.ProfessionalId == professionalId;
            
            var prescriptions = await _context.Prescriptions
                                        .Include(p => p.PrescriptionMedications)
                                        .Where(predicate)
                                        .OrderBy(x => x.CreatedDate)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if (!prescriptions.Any() || prescriptions == null)
            {
                _logger.LogError("Not Found {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {professionalId}",
                    "Prescription not found",
                    DateTime.UtcNow.AddHours(1));

                return null;
            }

            return new GetPrescriptionByProfessionalIdResult(
                        prescriptions: prescriptions.Select(p => new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.Name,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified)).ToList());
        }

        public async Task<GetAllPrescriptionsResult> GetAllPrescriptionsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            Expression<Func<Entities.Prescription, bool>> predicate = s => s.IsActive;

            if (!string.IsNullOrEmpty(searchParam))
            {
                var searchParamLower = searchParam.ToLower(); 
                predicate = s => s.IsActive && (
                    (s.Professional != null && (
                        (s.Professional.FirstName != null && s.Professional.FirstName.ToLower().Contains(searchParamLower)) ||
                        (s.Professional.MiddleName != null && s.Professional.MiddleName.ToLower().Contains(searchParamLower)) ||
                        (s.Professional.LastName != null && s.Professional.LastName.ToLower().Contains(searchParamLower))
                    )) ||
                    (s.Patient != null && (
                        (s.Patient.FirstName != null && s.Patient.FirstName.ToLower().Contains(searchParamLower)) ||
                        (s.Patient.MiddleName != null && s.Patient.MiddleName.ToLower().Contains(searchParamLower)) ||
                        (s.Patient.LastName != null && s.Patient.LastName.ToLower().Contains(searchParamLower))
                    )) ||
                    (s.Diagnosis != null && s.Diagnosis.ToLower().Contains(searchParamLower))
                );
            }

            var prescriptions = await _context.Prescriptions
                                        .Include(x => x.Professional)
                                        .Include(y => y.Patient)
                                        .Include(z => z.PrescriptionMedications)  
                                        .Where(predicate)
                                        .OrderBy(x => x.CreatedDate)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if (!prescriptions.Any() || prescriptions == null)
                return null;

            return new GetAllPrescriptionsResult(
                        prescriptions: prescriptions.Select(p => new PrescriptionDto(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            diagnosis: p.Diagnosis,
                            medications: p.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.Name,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),  
                            isActive: p.IsActive,
                            createdDate: p.CreatedDate,
                            dateModified: p.DateModified)).ToList());
        }
    }
}
