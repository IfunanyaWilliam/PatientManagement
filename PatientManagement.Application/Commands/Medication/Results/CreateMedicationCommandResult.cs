﻿
namespace PatientManagement.Application.Commands.Medication.Results
{
    using Common.Contracts;


    public class CreateMedicationCommandResult : ICommandResult
    {
        public CreateMedicationCommandResult(
            Guid id,
            string name,
            string description,
            bool isActive,
            DateTime createdDate,
            DateTime? dateModified)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedDate = createdDate;
            DateModified = dateModified;
        }


        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public bool IsActive { get; }
        public DateTime CreatedDate { get; }
        public DateTime? DateModified { get; }
    }
}
