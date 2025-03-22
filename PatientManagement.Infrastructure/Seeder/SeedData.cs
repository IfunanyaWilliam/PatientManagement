
namespace PatientManagement.Infrastructure.Seeder
{
    using Microsoft.AspNetCore.Identity;
    using DbContexts;
    using Entities;
    using PatientManagement.Common.Enums;
    using Microsoft.EntityFrameworkCore;
    using PatientManagement.Infrastructure.Services.Interfaces;
    using System.Data;

    public static class SeedData
    {
        public static async Task Seed(
            UserManager<Entities.ApplicationUser> userManager,
            AppDbContext dbContext,
            RoleManager<IdentityRole<Guid>> roleManager,
            IEncryptionService encryptionService)
        {
            dbContext.Database.EnsureCreated();
            await SeedPermissions(dbContext, encryptionService);
            await SeedRolesAndUsers(userManager, roleManager, dbContext, encryptionService);
            await SeedMedications(dbContext);
            await SeedPatient(userManager, dbContext);
            await SeedProfessional(userManager, dbContext);
        }

        private static async Task SeedPermissions(AppDbContext dbContext, IEncryptionService encryptionService)
        {
            if (!dbContext.Permissions.Any())
            {
                var permissions = new[]
                {
                    "CreatePatient",
                    "ViewMedicalRecords",
                    "ManagePatientRecords",
                    "ManageProfessionalRecords",
                    "DeleteMedicalRecords",
                    "ManageMedicalRecords",
                    "ManageUsers",
                    "CreateAdmin"
                };

                foreach (var permission in permissions)
                {
                    await dbContext.Permissions.AddAsync(new Permission
                    {
                        EncryptedName = encryptionService.Encrypt(permission)
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedRolesAndUsers(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            AppDbContext dbContext,
            IEncryptionService encryptionService)
        {

            var roles = new Dictionary<string, string[]>
            {
                { nameof(UserRole.Patient), new[] { "CreatePatient", "ViewMedicalRecords" } },

                { nameof(UserRole.Professional), new[] { "ManagePatientRecords", "ManageProfessionalRecords" }},

                { nameof(UserRole.Administrator), new[] { "ManageMedicalRecords",
                                                          "DeleteMedicalRecords", 
                                                          "ManageUsers" }},

                { nameof(UserRole.SuperAdministrator), new[] { "ManageMedicalRecords",
                                                               "DeleteMedicalRecords", 
                                                               "ManageUsers", 
                                                               "CreateAdmin"}}
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Key))
                {
                    var identityRole = new IdentityRole<Guid> { Name = role.Key };
                    await roleManager.CreateAsync(identityRole);

                    var permissions = await dbContext.Permissions
                        .Where(p => role.Value.Contains(encryptionService.Decrypt(p.EncryptedName)))
                        .ToListAsync();

                    foreach (var permission in permissions)
                    {
                        await dbContext.RolePermissions.AddAsync(new RolePermission
                        {
                            RoleId = identityRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            await dbContext.SaveChangesAsync();
            await SeedUser(userManager, "admin@abchealth.ng", UserRole.SuperAdministrator);
        }

        private static async Task SeedUser(
            UserManager<ApplicationUser> userManager,
            string email,
            UserRole userRole)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "Pa55word@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userRole.ToString());
                }
                else
                {
                    Console.WriteLine($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
        
        private static async Task SeedMedications(AppDbContext dbContext)
        {
            List<Entities.Medication> medications = new List<Entities.Medication>
            {
                new Entities.Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Paracetamol",
                    Description = "Pain reliever",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Entities.Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Diclofenac Xtra",
                    Description = "Pain reliever",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Entities.Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Paracetamol Extra",
                    Description = "Acute Pain reliever",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Entities.Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Vitamin C",
                    Description = "Mineral supplement",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Entities.Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Leonart",
                    Description = "Kills Plasmodium in blood stream",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                }
            };

            await dbContext.Medications.AddRangeAsync(medications);
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedPatient(
            UserManager<Entities.ApplicationUser> userManager,
            AppDbContext dbContext)
        {
            var patientUser = new Entities.ApplicationUser
            {
                Email = "mberede.dike@abc.com",
                UserName = "mberede.dike@abc.com",
                EmailConfirmed = true,
                CreatedDate = DateTime.UtcNow.AddHours(1)
            };

            string password = "Password!@1";

            var result = await userManager.CreateAsync(patientUser, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(patientUser, UserRole.Professional.ToString());
                var patient = new Entities.Patient
                {
                    ApplicationUserId = patientUser.Id,
                    Title = "Engr.",
                    FirstName = "Mberede",
                    MiddleName = "Nyiri",
                    LastName = "Dike",
                    PhoneNumber = "09087899887",
                    Age = 25,
                    IsActive = true,
                    IsDeleted = false,
                    Role = UserRole.Patient
                };

                await dbContext.Patients.AddAsync(patient);
                await dbContext.SaveChangesAsync();
            }
        }
        
        public static async Task SeedProfessional(
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext)
        {
            Entities.ApplicationUser professionalUser = new Entities.ApplicationUser
            {
                Email = "mberede.Amadike@abc.com",
                UserName = "mberede.Amadike@abc.com",
                EmailConfirmed = true,
                CreatedDate = DateTime.UtcNow.AddHours(1)
            };

            string password = "Password!@4";
            var result = await userManager.CreateAsync(professionalUser, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(professionalUser, UserRole.Professional.ToString());
                var professional = new Entities.Professional
                {
                    ApplicationUserId = professionalUser.Id,
                    Title = "Dr",
                    FirstName = "Mbere",
                    MiddleName = "Kaeji",
                    LastName = "Amadike",
                    PhoneNumber = "07098898767",
                    Age = 28,
                    Qualification = "MBBS",
                    License = "NG-MBBS-250914",
                    IsActive = true,
                    IsDeleted = false,
                    Role = UserRole.Professional,
                    ProfessionalStatus = ProfessionalStatus.Active
                };
                dbContext.Professionals.Add(professional);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
