
namespace PatientManagement.Application.Commands.Patient.Results
{
    using Interfaces.Commands;

    public class DeletePatientCommandResult : ICommandResult
    {
        public DeletePatientCommandResult(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public bool IsDeleted { get; }
    }
}
