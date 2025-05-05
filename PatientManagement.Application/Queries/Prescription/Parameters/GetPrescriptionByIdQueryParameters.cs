
namespace PatientManagement.Application.Queries.Prescription.Parameters
{
    using Interfaces.Queries;

    public class GetPrescriptionByIdQueryParameters : IQueryParameters
    {
        public GetPrescriptionByIdQueryParameters(
            Guid prescriptionId)
        {
            PrescriptionId = prescriptionId;
        }

        public Guid PrescriptionId { get; }
    }
}
