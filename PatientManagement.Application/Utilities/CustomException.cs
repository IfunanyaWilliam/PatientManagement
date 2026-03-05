
namespace PatientManagement.Application.Utilities
{
    using Microsoft.AspNetCore.Http;

    public class CustomException : Exception
    {
        public int StatusCode { get; }
        public int ResponseCode { get; }

        public CustomException(string message, int statusCode = StatusCodes.Status500InternalServerError, int responseCode = 99)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseCode = responseCode;
        }
    }
}
