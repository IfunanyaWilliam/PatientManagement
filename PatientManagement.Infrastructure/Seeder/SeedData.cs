
namespace PatientManagement.Infrastructure.Seeder
{
    using Microsoft.AspNetCore.Identity;
    using DbContexts;
    using Entities;
    using PatientManagement.Common.Enums;
    using Microsoft.EntityFrameworkCore;
    using PatientManagement.Infrastructure.Services.Interfaces;

    public static class SeedData
    {
        public static async Task Seed(
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext,
            RoleManager<IdentityRole<Guid>> roleManager,
            IEncryptionService encryptionService)
        {
            dbContext.Database.EnsureCreated();
            await SeedPermissions(dbContext, encryptionService);
            await SeedRolesAndUsers(userManager, roleManager, dbContext, encryptionService);
            await SeedEntities(dbContext);
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

        private static async Task SeedEntities(AppDbContext dbContext)
        {
            List<Medication> medications = new List<Medication>
            {
                new Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Paracetamol",
                    Description = "Pain reliever",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Paracetamol",
                    Description = "Pain reliever",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Paracetamol Extra",
                    Description = "Acute Pain reliever",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Medication
                {
                    Id = Guid.NewGuid(),
                    Name = "Vitamin C",
                    Description = "Mineral supplement",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow.AddHours(1),
                    DateModified = null
                },

                new Medication
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
    }
}
