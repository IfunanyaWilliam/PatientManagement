
namespace PatientManagement.Application.Queries.Professional.Parameters
{
    using Interfaces.Queries;

    public class GetProfessionalByIdQueryParameters : IQueryParameters
    {
        public GetProfessionalByIdQueryParameters(Guid professionalId)
        {
            ProfessionalId = professionalId;
        }

        public Guid ProfessionalId { get; }
    }
}
