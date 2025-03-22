
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using System.Linq;
    using System.Text.Json;
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Repositories.Interfaces;
    using Common.Utilities;
    using Domain.Patient;
    using Common.Enums;
    using DbContexts;
          

    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Entities.ApplicationUser> _userManager;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(
            AppDbContext context,
            UserManager<Entities.ApplicationUser> userManager,

            ILogger<PatientRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Patient> CreatePatientAsync(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            CancellationToken cancellationToken = default)
        {
            if(applicationUserId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            if(string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(middleName) 
                || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);
            }

            if(age < 0 || age > 150)
                throw new CustomException($"Age range is invalid", StatusCodes.Status400BadRequest);

            var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
            if (user is null)
                throw new CustomException($"UserId {applicationUserId} not found", StatusCodes.Status404NotFound);

            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == applicationUserId, 
                cancellationToken);
            if(existingPatient is not null)
            {
                throw new CustomException($"Patient with UserId: {applicationUserId} already exist", StatusCodes.Status400BadRequest);
            }

            var patient = new Entities.Patient
            {
                ApplicationUserId = applicationUserId,
                Title = title,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Age = age,
                IsActive = true,
                IsDeleted = false,
                Role = UserRole.Patient
            };

            await _context.Patients.AddAsync(patient);
            var result = await _context.SaveChangesAsync();
            
            if(result > 0)
            {
                return new Patient(
                    id: patient.Id,
                    applicationUserId: patient.ApplicationUserId,
                    title: title,
                    firstName: firstName,
                    middleName: patient.MiddleName,
                    lastName: patient.LastName,
                    phoneNumber: patient.PhoneNumber,
                    age: patient.Age,
                    email: user.Email,
                    isActive: patient.IsActive,
                    userRole: patient.Role.ToString(),
                    dateCreated: patient.DateCreated,
                    dateModified: patient.DateModified);
            }

            _logger.LogError($"Patient could not be saved, data => {JsonSerializer.Serialize(patient)}");
            throw new CustomException("Patient could not be created, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Patient> UpdatePatientAsync(
            Guid id,
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            CancellationToken cancellationToken = default)
        {
            if (applicationUserId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(title)
                || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(title))
            {
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);
            }

            if (age < 0 || age > 150)
                throw new CustomException($"Age range is invalid", StatusCodes.Status400BadRequest);

            var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
            if (user is null)
                throw new CustomException($"UserId {applicationUserId} not found", StatusCodes.Status404NotFound);

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (patient is null)
            {
                throw new CustomException($"Patient with Id {id} not found", StatusCodes.Status404NotFound);
            }

            patient.Title = title is null ? patient.Title : title;
            patient.FirstName = firstName is null ? patient.FirstName : firstName;
            patient.MiddleName = middleName is null ? patient.MiddleName : middleName;
            patient.LastName = lastName is null ? patient.LastName : lastName;
            patient.PhoneNumber = phoneNumber is null ? patient.PhoneNumber : phoneNumber;
            patient.Age = age;
            patient.DateModified = DateTime.UtcNow.AddHours(1);
             
            _context.Patients.Update(patient);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Patient(
                    id: patient.Id,
                    applicationUserId: patient.ApplicationUserId,
                    title: title,
                    firstName: firstName,
                    middleName: patient.MiddleName,
                    lastName: patient.LastName,
                    phoneNumber: patient.PhoneNumber,
                    age: patient.Age,
                    email: user.Email,
                    isActive: patient.IsActive,
                    userRole: patient.Role.ToString(),
                    dateCreated: patient.DateCreated,
                    dateModified: patient.DateModified);
            }

            _logger.LogError($"Patient could not be updated, data => {JsonSerializer.Serialize(patient)}");
            throw new CustomException("Patient could not be updated, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Patient> GetPatientAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && x.IsActive, cancellationToken);

            if (patient == null)
            {
                _logger.LogError("Not Found {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {id}",
                    "Patient not found",
                    DateTime.UtcNow.AddHours(1));

                throw new CustomException($"Patient with Id: {id} Not found", StatusCodes.Status404NotFound);
            }

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(p => p.Id == patient.ApplicationUserId, cancellationToken);

            if (user == null)
            {
                _logger.LogError("Not Found {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {id}",
                    "User that corresponds to patient not found",
                    DateTime.UtcNow.AddHours(1));

                throw new CustomException($"Corresponding user not found for patient with Id {id}", StatusCodes.Status404NotFound);
            }

            return new Patient(
                id: id,
                applicationUserId: patient.ApplicationUserId,
                title: patient.Title,
                firstName: patient.FirstName,
                middleName: patient.MiddleName,
                lastName: patient.LastName,
                phoneNumber: patient.PhoneNumber,
                age: patient.Age,
                email: user.Email,
                isActive: patient.IsActive,
                userRole: patient.Role.ToString(),
                dateCreated: patient.DateCreated,
                dateModified: patient.DateModified);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            Expression<Func<Entities.Patient, bool>> predicate = s => s.IsActive;

            if (!string.IsNullOrWhiteSpace(searchParam))
            {
                var searchParamLower = searchParam.ToLower();
                predicate = s => s.IsActive && ((s.FirstName != null && s.FirstName.ToLower().Contains(searchParamLower)) ||
                                                (s.MiddleName != null && s.MiddleName.ToLower().Contains(searchParamLower)) ||
                                                (s.LastName != null && s.LastName.ToLower().Contains(searchParamLower)));
            }

            var prescriptions = await _context.Patients
                                        .Include(a => a.ApplicationUser)
                                        .Where(predicate)
                                        .OrderBy(x => x.DateCreated)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if (prescriptions is null)
                return new List<Patient>();

            return prescriptions.Select(p => new Patient(
                id: p.Id,
                applicationUserId: p.ApplicationUserId,
                title: p.Title,
                firstName: p.FirstName,
                middleName: p.MiddleName,
                lastName: p.LastName,
                phoneNumber: p.PhoneNumber,
                age: p.Age,
                email: p.ApplicationUser.Email,
                isActive: p.IsActive,
                userRole: p.ApplicationUser.Role.ToString(),
                dateCreated: p.DateCreated,
                dateModified: p.DateModified));
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                _logger.LogError("Not Found {@Param}, {@Error}, {@DateTimeUtc}",
                    $"id: {id}",
                    "DeletePatient: Patient not found",
                    DateTime.UtcNow.AddHours(1));

                throw new CustomException($"Patient with Id: {id} Not found", StatusCodes.Status404NotFound);
            }

            patient.IsDeleted = true;
            patient.DateModified = DateTime.UtcNow;

            _context.Update(patient);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
