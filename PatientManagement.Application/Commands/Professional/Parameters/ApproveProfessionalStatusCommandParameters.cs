
namespace PatientManagement.Application.Commands.Professional.Parameters
{
    using Common.Contracts;

    public class ApproveProfessionalStatusCommandParameters : ICommand
    {
        public ApproveProfessionalStatusCommandParameters(Guid professionalId)
        {
            ProfessionalId = professionalId;
        }

        public Guid ProfessionalId { get; set; }
    }
}
