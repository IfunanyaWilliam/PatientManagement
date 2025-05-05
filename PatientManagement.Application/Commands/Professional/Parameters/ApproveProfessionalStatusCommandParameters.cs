
namespace PatientManagement.Application.Commands.Professional.Parameters
{
    using Interfaces.Commands;

    public class ApproveProfessionalStatusCommandParameters : ICommand
    {
        public ApproveProfessionalStatusCommandParameters(Guid professionalId)
        {
            ProfessionalId = professionalId;
        }

        public Guid ProfessionalId { get; set; }
    }
}
