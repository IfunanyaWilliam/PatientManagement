﻿
namespace PatientManagement.Domain.Prescription
{
    public class Medication
    {
        public Medication(
            Guid id,
            string? name,
            string? description,
            bool isActive,
            DateTime dateCreated,
            DateTime? dateModified)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
