using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Infrastructure.Entities;
using System.Security;

namespace PatientManagement.Infrastructure.DbContexts
{
    public class AppDbContext : IdentityDbContext
        <ApplicationUser,
         IdentityRole<Guid>,
         Guid,
         IdentityUserClaim<Guid>,
         IdentityUserRole<Guid>,
         IdentityUserLogin<Guid>,
         IdentityRoleClaim<Guid>,
         IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.ApplicationUser)
                .WithOne() 
                .HasForeignKey<Patient>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Professional>()
                .HasOne(p => p.ApplicationUser)
                .WithOne()
                .HasForeignKey<Professional>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany()
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Professional)
                .WithMany()
                .HasForeignKey(p => p.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExternalLogin>()
            .HasOne(e => e.ApplicationUser)
            .WithMany(u => u.ExternalLogins)
            .HasForeignKey(e => e.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);


            // Primary Key - Already has clustered index by default
            modelBuilder.Entity<ApplicationUser>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<ExternalLogin>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Patient>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Professional>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Prescription>()
                .HasKey(p => p.Id);

            // ==================== ADDITIONAL INDEXES ====================

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.IsDeleted);

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Role);

            // ExternalLogin Indexes
            modelBuilder.Entity<ExternalLogin>()
                .HasIndex(e => new { e.ProviderUserId, e.Provider })
                .IsUnique();

            modelBuilder.Entity<ExternalLogin>()
                .HasIndex(e => e.ApplicationUserId);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.ApplicationUserId)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.PhoneNumber);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.IsDeleted);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.IsActive);

            modelBuilder.Entity<Professional>()
                .HasIndex(p => p.ApplicationUserId)
                .IsUnique();

            modelBuilder.Entity<Professional>()
                .HasIndex(p => p.License)
                .IsUnique();

            modelBuilder.Entity<Professional>()
                .HasIndex(p => p.IsDeleted);
            modelBuilder.Entity<Professional>()
                .HasIndex(p => p.IsActive);

            modelBuilder.Entity<Professional>()
                .HasIndex(p => p.ProfessionalStatus);

            modelBuilder.Entity<Prescription>()
                .HasIndex(p => p.PatientId);

            modelBuilder.Entity<Prescription>()
                .HasIndex(p => p.ProfessionalId);

            modelBuilder.Entity<Prescription>()
                .HasIndex(p => p.IsActive);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PrescriptionMedication> PrescriptionMedications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ExternalLogin> ExternalLogins { get; set; }
    }
}
