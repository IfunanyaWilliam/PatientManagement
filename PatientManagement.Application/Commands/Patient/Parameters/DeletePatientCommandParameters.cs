
namespace PatientManagement.Application.Commands.Patient.Parameters
{
    using Common.Contracts;
    public class DeletePatientCommandParameters : ICommand
    {
        public DeletePatientCommandParameters(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
