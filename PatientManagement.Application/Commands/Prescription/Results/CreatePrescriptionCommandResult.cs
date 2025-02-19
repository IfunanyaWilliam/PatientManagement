﻿
namespace PatientManagement.Application.Commands.Prescription.Results
{
    using Common.Contracts;
    using Common.Dto;

    public class CreatePrescriptionCommandResult : ICommandResult
    {
        public CreatePrescriptionCommandResult(
            Guid id,
            Guid patientId,
            Guid professionalId,
            string? diagnosis,
            List<PrescribedMedication>? medications,
            bool isActive,
            DateTime createdDate)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            Diagnosis = diagnosis;
            Medications = medications;
            IsActive = isActive;
            CreatedDate = createdDate;
        }

        public Guid Id { get; set; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public string? Diagnosis { get; }
        public List<PrescribedMedication>? Medications { get; }
        public bool IsActive { get; }
        public DateTime CreatedDate { get; }
    }
}
