
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
                CreatedDate = DateTime.UtcNow,
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
                    createdDate: medication.CreatedDate,
                    dateModified: medication.DateModified);
            }

            _logger.LogError($"Medication could not be saved to db, data => {JsonSerializer.Serialize(medication)}");
            throw new CustomException("Medication could not be created, try again later", StatusCodes.Status500InternalServerError);
        }



    }
}
