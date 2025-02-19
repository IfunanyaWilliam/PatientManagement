
namespace PatientManagement.Application.Queries.Prescription.Parameters
{
    using Common.Contracts;

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
