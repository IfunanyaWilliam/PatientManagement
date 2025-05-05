
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using System.Text.Json;
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.DbContexts;
    using Domain.Professional;
    using Domain.ApplicationUser;
    using Application.Utilities;
    using Application.Interfaces.Repositories;

    public class ProfessionalRepository : IProfessionalRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Entities.ApplicationUser> _userManager;
        private readonly ILogger<ProfessionalRepository> _logger;


        public ProfessionalRepository(
            AppDbContext context,
            UserManager<Entities.ApplicationUser> userManager,
            ILogger<ProfessionalRepository> logger) 
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Professional> CreateProfessionalAsync(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            string? qualification,
            string? license,
            UserRole userRole,
            CancellationToken cancellationToken = default)
        {
            if (applicationUserId == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(middleName)
                || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(qualification)
                || string.IsNullOrWhiteSpace(license))
            {
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);
            }

            if (age < 0 || age > 70)
                throw new CustomException($"Age range is invalid", StatusCodes.Status400BadRequest);

            var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
            if (user is null)
                throw new CustomException($"UserId {applicationUserId} not found", StatusCodes.Status404NotFound);

            var existingProfessional = await _context.Professionals.FirstOrDefaultAsync(p => p.ApplicationUserId == applicationUserId,
                cancellationToken);
            if (existingProfessional is not null)
            {
                throw new CustomException($"Professional with UserId already exist", StatusCodes.Status400BadRequest);
            }

            var professional = new Entities.Professional
            {
                ApplicationUserId = applicationUserId,
                Title = title,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Age = age,
                Qualification = qualification,
                License = license,
                IsActive = false,
                IsDeleted = false,
                Role = userRole,
                ProfessionalStatus = ProfessionalStatus.PendingBackgroundCheck
            };

            await _context.Professionals.AddAsync(professional);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Professional(
                    id: professional.Id,
                    applicationUserId: professional.ApplicationUserId,
                    title: professional.Title,
                    firstName: professional?.FirstName,
                    middleName: professional?.MiddleName,
                    lastName: professional?.LastName,
                    phoneNumber: professional?.PhoneNumber,
                    age: professional.Age,
                    qualification: professional.Qualification,
                    license: professional?.License,
                    email: user.Email,
                    isActive: professional.IsActive,
                    userRole: professional.Role.ToString(),
                    professionalStatus: professional.ProfessionalStatus.ToString(),
                dateCreated: professional.DateCreated,
                dateModified: professional.DateModified);
            }

            _logger.LogError($"Professional could not be saved to db, data => {JsonSerializer.Serialize(professional)}");
            throw new CustomException("Professional could not be created, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Professional> ApproveProfessionalStatusAsync(
            Guid professionalId)
        {
            if(professionalId == Guid.Empty)
                throw new CustomException($"Invalid Parameter", StatusCodes.Status400BadRequest);

            var professional = await _context.Professionals.FindAsync( professionalId);
            if(professional  == null)
                throw new CustomException($"Professional with Id {professionalId} not found", StatusCodes.Status404NotFound);

            var user = await _userManager.FindByIdAsync(professional.ApplicationUserId.ToString());
            if (user is null)
                throw new CustomException($"User associated with professioalId {professionalId} not found", StatusCodes.Status404NotFound);

            professional.IsActive = true;
            professional.ProfessionalStatus = ProfessionalStatus.Active;
            professional.DateModified = DateTime.UtcNow.AddHours(1);
            _context.Update(professional);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new Professional(
                    id: professional.Id,
                    applicationUserId: professional.ApplicationUserId,
                    title: professional.Title,
                    firstName: professional?.FirstName,
                    middleName: professional?.MiddleName,
                    lastName: professional?.LastName,
                    phoneNumber: professional?.PhoneNumber,
                    age: professional.Age,
                    qualification: professional?.Qualification,
                    license: professional?.License,
                    email: user.Email,
                    isActive: professional.IsActive,
                    userRole: professional.Role.ToString(),
                    professionalStatus: professional.ProfessionalStatus.ToString(),
                    dateCreated: professional.DateCreated,
                    dateModified: professional?.DateModified);
            }

            _logger.LogError($"ProfessionalStatus could not be updated , data => {JsonSerializer.Serialize(professionalId)}");
            throw new CustomException("ProfessionalStatus could not be updated, try again later", StatusCodes.Status500InternalServerError);
        }

        public async Task<Professional> GetProfessionalByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new CustomException($"Invalid input", StatusCodes.Status400BadRequest);

            var professional = await _context.Professionals.FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);

            if(professional == null || professional.IsDeleted)
                throw new CustomException($"Professional with Id {id} not found", StatusCodes.Status404NotFound);

            var user = await _userManager.FindByIdAsync(professional.ApplicationUserId.ToString());
            if (user is null)
                throw new CustomException($"User associated with professioalId {id} not found", StatusCodes.Status404NotFound);

            if (!professional.IsActive)
                throw new CustomException($"Professional with Id {id} is not active, contact Admin for support", StatusCodes.Status400BadRequest);

            //Don't return suspended message to user
            if(professional.ProfessionalStatus == ProfessionalStatus.Suspended)
                throw new CustomException($"Professional with Id {id} is not active, contact Admin for support", StatusCodes.Status400BadRequest);

            return new Professional(
                id: professional.Id,
                applicationUserId: professional.ApplicationUserId,
                title: professional.Title,
                firstName: professional.FirstName,
                middleName: professional.MiddleName,
                lastName: professional.LastName,
                phoneNumber: professional.PhoneNumber,
                age: professional.Age,
                qualification: professional.Qualification,
                license: professional.License,
                email: user.Email,
                isActive: professional.IsActive,
                userRole: professional.Role.ToString(),
                professionalStatus: professional.ProfessionalStatus.ToString(),
                dateCreated: professional.DateCreated,
                dateModified: professional.DateModified);
        }

        public async Task<IEnumerable<Professional>> GetAllProfessionalsAsync(
            int pageNumber,
            int pageSize,
            string searchParam,
            CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            Expression<Func<Entities.Professional, bool>> predicate = s => !s.IsDeleted;

            if (!string.IsNullOrWhiteSpace(searchParam))
            {
                var searchParamLower = searchParam.ToLower();

                predicate = s =>    (s.FirstName != null && s.FirstName.ToLower().Contains(searchParamLower)) ||
                                    (s.MiddleName != null && s.MiddleName.ToLower().Contains(searchParamLower)) ||
                                    (s.LastName != null && s.LastName.ToLower().Contains(searchParamLower)) ||
                                    (s.Qualification != null && s.Qualification.ToLower().Contains(searchParamLower));
            }

            var professionals = await _context.Professionals
                                        .Include(y => y.ApplicationUser)
                                        .Where(predicate)
                                        .OrderBy(x => x.DateCreated)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToListAsync(cancellationToken);

            if (!professionals.Any() || professionals is null)
                return null;

            return professionals.Select(p => new Professional(
                        id: p.Id,
                        applicationUserId: p.ApplicationUserId,
                        title: p.Title,
                        firstName: p.FirstName,
                        middleName: p.MiddleName,
                        lastName: p.LastName,
                        phoneNumber: p.PhoneNumber,
                        age: p.Age,
                        qualification: p.Qualification,
                        license: p.License,
                        email: p.ApplicationUser.Email,
                        isActive: p.IsActive,
                        userRole: p.Role.ToString(),
                        professionalStatus: p.ProfessionalStatus.ToString(),
                        dateCreated: p.DateCreated,
                        dateModified: p.DateModified));
        }
    }
}
