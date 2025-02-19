
namespace PatientManagement.Application.Commands.Patient.Results
{
    using Common.Contracts;
    public class DeletePatientCommandResult : ICommandResult
    {
        public DeletePatientCommandResult(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public bool IsDeleted { get; }
    }
}
