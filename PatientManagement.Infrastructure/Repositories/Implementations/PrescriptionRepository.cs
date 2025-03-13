
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using System.Text.Json;
    using System.Linq;
    using Common.Results;
    using DbContexts;
    using Common.Utilities;
    using Interfaces;
    using Common.Dto;
    using Domain.Patient;
    using Domain.Prescription;
    using Domain.Professional;
    using Common.Parameters;

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


        public async Task<Prescription> CreatePrescriptionAsync(
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string diagnosis,
            IEnumerable<MedicationParameters> medications,
            CancellationToken cancellationToken)
        {
            if (patientId == Guid.Empty || professionalId == Guid.Empty
                || string.IsNullOrEmpty(diagnosis) || medications.Count() <= 0)
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

            var medicationIds = medications.Select(m => m.MedicationId).ToList();

            var existingMedications = await _context.Medications
                .Where(m => medicationIds.Contains(m.Id) && m.IsActive)
                .ToListAsync();

            var medicationIdsNotFound = medicationIds.Except(existingMedications.Select(m => m.Id));

            if (medicationIdsNotFound.Any())
            {
                throw new CustomException(
                    "Medications not found, medicationIds: " + string.Join(",", medicationIdsNotFound),
                    StatusCodes.Status400BadRequest);
            }

            var prescription = new Entities.Prescription
            {
                PatientId = patientId,
                ProfessionalId = professionalId,
                Symptoms = symptoms,
                Diagnosis = diagnosis,
                IsActive = true
            };

            await _context.AddAsync(prescription, cancellationToken);

            var prescriptionMedications = existingMedications.Select(m => new Entities.PrescriptionMedication
            {
                PatientId = patientId,
                ProfessionalId = professionalId,
                PrescriptionId = prescription.Id,
                MedicationId = m.Id,
                MedicationName = m.Name,
                Dosage = medications.FirstOrDefault(m => m.MedicationId == m.MedicationId).Dosage,
                Instruction = medications.FirstOrDefault(m => m.MedicationId == m.MedicationId).Instruction,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
            }).ToList();

            await _context.PrescriptionMedications.AddRangeAsync(prescriptionMedications, cancellationToken);

            prescription.PrescriptionMedications = prescriptionMedications;
            var result = await _context.SaveChangesAsync(cancellationToken);

            if(result > 0)
            {
                return new Prescription(
                    id: prescription.Id,
                    patientId: prescription.PatientId,
                    professionalId: prescription.ProfessionalId,
                    symptoms: prescription.Symptoms,
                    diagnosis: prescription.Diagnosis,
                    isActive: prescription.IsActive,
                    dateCreated: prescription.DateCreated,
                    dateModified: prescription.DateModified);
            }

            _logger.LogError($"Prescription could not be saved to db, data => {JsonSerializer.Serialize(prescription)}");
            throw new CustomException("Prescription could not be created, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Prescription> UpdatePrescriptionAsync(
            Guid prescriptionId,
            Guid patientId,
            Guid professionalId,
            string symptoms,
            string diagnosis,
            IEnumerable<MedicationParameters> medications,
            CancellationToken cancellationToken)
        {
            if (patientId == Guid.Empty || professionalId == Guid.Empty
                || string.IsNullOrEmpty(diagnosis) || medications.Count() <= 0)
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
                .Include(p => p.PrescriptionMedications.Where(p => p.PrescriptionId == prescriptionId))
                .FirstOrDefaultAsync(p => 
                    p.Id == prescriptionId &&
                    p.PatientId == patientId && 
                    p.ProfessionalId == professionalId, 
                    cancellationToken);

            if (prescription is null)
                throw new CustomException($"Prescription with Id {prescriptionId} Not Found", StatusCodes.Status404NotFound);

            var medicationIds = medications.Select(m => m.MedicationId).ToList();

            var existingMedications = await _context.Medications
                .Where(m => medicationIds.Contains(m.Id) && m.IsActive)
                .ToListAsync();

            var medicationIdsNotFound = medicationIds.Except(existingMedications.Select(m => m.Id));

            if (medicationIdsNotFound.Any())
            {
                throw new CustomException(
                    "Medications not found, medicationIds: " + string.Join(",", medicationIdsNotFound),
                    StatusCodes.Status400BadRequest);
            }

            var existingPrescriptionMedications = prescription.PrescriptionMedications;

            var existingPrescriptionMedicationIds = existingPrescriptionMedications
                          .Select(pm => pm.MedicationId)
                          .ToList();

            IEnumerable<Guid> medicationIdsToRemove = existingPrescriptionMedicationIds
                .Except(existingMedications.Select(m => m.Id));

            //Remove all prescription medications that match medication Id to remove.
            existingPrescriptionMedications
                    .Where(pm => !medicationIdsToRemove.Contains(pm.MedicationId))
                    .ToList()
                    .ForEach(pm => pm.IsActive = true);

            var newPrescriptionMedications = new List<Entities.PrescriptionMedication>();
            foreach (var medication in medications)
            {
                if (!existingPrescriptionMedications.Any(m => m.MedicationId == medication.MedicationId))
                {
                    var newMedication = await _context.Medications.FirstOrDefaultAsync(m => m.Id == medication.MedicationId);
                    var newPrescriptionMedication = new Entities.PrescriptionMedication
                    {
                        PatientId = patientId,
                        MedicationId = medication.MedicationId,
                        PrescriptionId = prescription.Id,
                        ProfessionalId = professionalId,
                        MedicationName = newMedication.Name,
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
                    item.IsActive = item.IsActive;
                    item.DateModified = DateTime.UtcNow.AddHours(1);
                }
            }

            if(existingPrescriptionMedications.Count > 0)
                _context.PrescriptionMedications.UpdateRange(existingPrescriptionMedications);

            var allMedications = newPrescriptionMedications.Concat(existingPrescriptionMedications).ToList();

            prescription.Symptoms = symptoms ?? prescription.Symptoms;
            prescription.Diagnosis = diagnosis ?? prescription.Diagnosis;
            prescription.DateModified = DateTime.UtcNow.AddHours(1);
            prescription.PrescriptionMedications = allMedications;
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Prescription(
                    id: prescription.Id,
                    patientId: prescription.PatientId,
                    professionalId: prescription.ProfessionalId,
                    symptoms: prescription.Symptoms,
                    diagnosis: prescription.Diagnosis,
                    isActive: prescription.IsActive,
                    dateCreated: prescription.DateCreated,
                    dateModified: prescription.DateModified);
            }

            _logger.LogError($"Medications could not be updated to db, data => {JsonSerializer.Serialize(prescription)}");
            throw new CustomException("Prescription could not be Updated, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<PrescriptionMedication> GetPrescriptionByIdAsync(
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

            return new PrescriptionMedication(
                id: prescription.Id,
                patientId: prescription.PatientId,
                professionalId: prescription.ProfessionalId,
                prescriptionId: prescription.Id,
                symptoms: prescription.Symptoms,
                diagnosis: prescription.Diagnosis,
                medications: prescription.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.MedicationName,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                isActive: prescription.IsActive,
                dateCreated: prescription.DateCreated,
                dateModified: prescription.DateModified);
        }

        public async Task<IEnumerable<PrescriptionMedication>> GetPrescriptionsByPatientIdAsync(
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
                                        .OrderBy(x => x.DateCreated)
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

            return  prescriptions.Select(p => new PrescriptionMedication(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            prescriptionId: p.Id,
                            symptoms: p.Symptoms,
                            diagnosis: p.Diagnosis,
                            medications: p.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.MedicationName,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                            isActive: p.IsActive,
                            dateCreated: p.DateCreated,
                            dateModified: p.DateModified)).ToList();
        }

        public async Task<IEnumerable<PrescriptionMedication>> GetPrescriptionByProfessionalIdAsync(
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
                                        .OrderBy(x => x.DateCreated)
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

            return prescriptions.Select(p => new PrescriptionMedication(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            prescriptionId: p.Id,
                            symptoms: p.Symptoms,
                            diagnosis: p.Diagnosis,
                            medications: p.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.MedicationName,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),
                            isActive: p.IsActive,
                            dateCreated: p.DateCreated,
                            dateModified: p.DateModified));
        }

        //Implemnt GetPrescriptionsByMedicationId => return all prescriptionMedications where medicationId occurs
        //request => medicationId, int pageNumber, int pageSize
        //response => IEnumerable<PrescriptionMedication> 

        public async Task<IEnumerable<PrescriptionMedication>> GetAllPrescriptionsAsync(
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
                                        .OrderBy(x => x.DateCreated)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if (!prescriptions.Any() || prescriptions == null)
                return null;

            return prescriptions.Select(p => new PrescriptionMedication(
                            id: p.Id,
                            patientId: p.PatientId,
                            professionalId: p.ProfessionalId,
                            prescriptionId: p.Id,
                            symptoms: p.Symptoms,
                            diagnosis: p.Diagnosis,
                            medications: p.PrescriptionMedications?.Select(m => new PrescribedMedication(
                                medicationId: m.Id,
                                name: m.MedicationName,
                                dosage: m.Dosage,
                                instruction: m.Instruction,
                                isActive: m.IsActive)).ToList() ?? new List<PrescribedMedication>(),  
                            isActive: p.IsActive,
                            dateCreated: p.DateCreated,
                            dateModified: p.DateModified));
        }
    }
}
