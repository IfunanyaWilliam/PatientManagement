﻿namespace PatientManagement.Api.Results
{
    public class DeletePatientResult
    {
        public DeletePatientResult(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public bool IsDeleted { get; }
    }
}
