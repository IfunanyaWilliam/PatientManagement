
namespace PatientManagement.Application.Queries.Professional.Parameters
{
    using Common.Contracts;

    public class GetProfessionalByIdQueryParameters : IQueryParameters
    {
        public GetProfessionalByIdQueryParameters(Guid professionalId)
        {
            ProfessionalId = professionalId;
        }

        public Guid ProfessionalId { get; }
    }
}
