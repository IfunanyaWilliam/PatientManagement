﻿using PatientManagement.Common.Dto;

namespace PatientManagement.Common.Results
{
    public class GetPrescriptionResult
    {
        public GetPrescriptionResult(
            Guid id,
            Guid patientId,
            Guid professionalId,
            Guid prescriptionId,
            string? diagnosis,
            IEnumerable<PrescribedMedication>? medications,
            bool isActive,
            DateTime dateCreated,
            DateTime? dateModified)
        {
            Id = id;
            PatientId = patientId;
            ProfessionalId = professionalId;
            PrescriptionId = prescriptionId;
            Diagnosis = diagnosis;
            Medications = medications;
            IsActive = isActive;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Guid Id { get; }
        public Guid PatientId { get; }
        public Guid ProfessionalId { get; }
        public Guid PrescriptionId { get; }
        public string? Diagnosis { get; }
        public IEnumerable<PrescribedMedication>? Medications { get; }
        public bool IsActive { get; }
        public DateTime DateCreated { get; }
        public DateTime? DateModified { get; }
    }
}
