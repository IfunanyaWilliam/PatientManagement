﻿
namespace PatientManagement.Infrastructure.Entities
{
    using Common.Enums;

    public class Patient
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } 
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime DateModified { get; set; }
    }
}
