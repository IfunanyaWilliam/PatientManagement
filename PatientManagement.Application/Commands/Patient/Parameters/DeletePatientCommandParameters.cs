
namespace PatientManagement.Application.Commands.Patient.Parameters
{
    using Interfaces.Commands;

    public class DeletePatientCommandParameters : ICommand
    {
        public DeletePatientCommandParameters(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
