
namespace PatientManagement.Application.Utilities
{
    using Microsoft.AspNetCore.Http;

    public class CustomException : Exception
    {
        public int StatusCode { get; }

        public CustomException(string message, int statusCode = StatusCodes.Status500InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
