
namespace PatientManagement.Common.Results
{
    public class GetAllProfessionalsResult
    {
        public GetAllProfessionalsResult(IEnumerable<GetProfessionalResult> professionals)
        {
            Professionals = professionals;
        }

        public IEnumerable<GetProfessionalResult> Professionals { get; }
    }
}
