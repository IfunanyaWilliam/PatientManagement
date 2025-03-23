
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using DbContexts;
    using Common.Utilities;
    using Domain.Prescription;
    using Interfaces;
    using System.Linq.Expressions;
    using System.Linq;

    public class MedicationRepository : IMedicationRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MedicationRepository> _logger;


        public MedicationRepository(
            AppDbContext context,
            ILogger<MedicationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Medication> CreateMedicationAsync(
            string name,
            string description,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name)  || string.IsNullOrWhiteSpace(description))
            {
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);
            }

            var trimName = name.Trim();
            var trimDescription = description.Trim();
            var existingMedication = await _context.Medications.FirstOrDefaultAsync(m => m.Name == trimName && m.IsActive, cancellationToken);

            if(existingMedication != null)
                throw new CustomException($"Medication '{trimName}' with Id: {existingMedication.Id} already exists", StatusCodes.Status400BadRequest);

            var medication = new Entities.Medication
            {
                Name = trimName,
                Description = trimDescription,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
            };

            await _context.Medications.AddAsync(medication);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return new Medication(
                    id: medication.Id,
                    name: medication.Name,
                    isActive:medication.IsActive,
                    description: medication.Description,
                    dateCreated: medication.DateCreated,
                    dateModified: medication.DateModified);
            }

            _logger.LogError($"Medication could not be saved to db, data => {JsonSerializer.Serialize(medication)}");
            throw new CustomException("Medication could not be created, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Medication> UpdateMedicationAsync(
            Guid medicationId,
            string name,
            string description,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
            {
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);
            }

            var existingMedication = await _context.Medications.FirstOrDefaultAsync(m => m.Id == medicationId && m.IsActive, cancellationToken);

            if (existingMedication == null)
                throw new CustomException($"Medication with Id: {medicationId} Not Found", StatusCodes.Status404NotFound);

            existingMedication.Name = name.Trim();
            existingMedication.Description = description.Trim();
            existingMedication.DateModified = DateTime.UtcNow;
            _context.Medications.Update(existingMedication);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Medication(
                    id: existingMedication.Id,
                    name: existingMedication.Name,
                    isActive: existingMedication.IsActive,
                    description: existingMedication.Description,
                    dateCreated: existingMedication.DateCreated,
                    dateModified: existingMedication.DateModified);
            }

            _logger.LogError($"Medication could not be saved to db, data => {JsonSerializer.Serialize(existingMedication)}");
            throw new CustomException("Medication could not be updated, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Medication> GetMedicationByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var medication = await _context.Medications.FirstOrDefaultAsync(m => m.Id == id && m.IsActive, cancellationToken);

            if(medication == null)
                throw new CustomException($"Professional with Id {id} not found", StatusCodes.Status404NotFound);

            return new Medication(
                id: medication.Id,
                name: medication.Name,
                isActive: medication.IsActive,
                description: medication.Description,
                dateCreated: medication.DateCreated,
                dateModified: medication.DateModified);
        }

        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            Expression<Func<Entities.Medication, bool>> predicate = s => s.IsActive;

            if (!string.IsNullOrWhiteSpace(searchParam))
            {
                var searchParamLower = searchParam.Trim().ToLower();

                predicate = s => (s.Name != null && s.Name.ToLower().Contains(searchParamLower) ||
                                 (s.Description != null && s.Description.ToLower().Contains(searchParamLower)));
            }

            var medications = await _context.Medications
                                        .Where(predicate)
                                        .OrderBy(x => x.DateCreated)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if(medications is null || !medications.Any())
            {
                return new List<Medication>();
            }

            return medications.Select(m => new Medication(
                id: m.Id,
                name: m.Name,
                isActive: m.IsActive,
                description: m.Description,
                dateCreated: m.DateCreated,
                dateModified: m.DateModified));
        }
    }
}
