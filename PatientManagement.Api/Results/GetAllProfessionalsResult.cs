﻿
namespace PatientManagement.Api.Results
{
    public class GetAllProfessionalsResult
    {
        public GetAllProfessionalsResult(IEnumerable<GetProfessionalsResult> professionals)
        {
            Professionals = professionals;
        }

        public IEnumerable<GetProfessionalsResult> Professionals { get; }
    }
}
