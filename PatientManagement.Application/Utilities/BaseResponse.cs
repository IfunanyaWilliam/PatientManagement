
namespace PatientManagement.Application.Utilities
{
    using Microsoft.AspNetCore.Http;

    public class BaseResponse<T>
    {
        public bool IsSuccess { get; init; }
        public int ResponseCode { get; init; }
        public T? Data { get; init; }
        public string? Message { get; init; }
        public IReadOnlyList<string>? Errors { get; init; }

        private BaseResponse() { }

        public static BaseResponse<T> Success(T data, string? message = null, int responseCode = StatusCodes.Status200OK) =>
            new()
            {
                IsSuccess = true,
                ResponseCode = responseCode,
                Message = message,
                Data = data
            };

        public static BaseResponse<T> Fail(string error, string message, int responseCode = StatusCodes.Status400BadRequest, T? data = default) =>
        new()
        {
            IsSuccess = false,
            ResponseCode = responseCode,
            Message = message ?? null,
            Data = data,
            Errors = [error]
        };

        public static BaseResponse<T> Fail(IEnumerable<string> errors, int responseCode = StatusCodes.Status400BadRequest, T? data = default) =>
        new()
        {
            IsSuccess = false,
            ResponseCode = responseCode,
            Data = data,
            Errors = errors.ToList().AsReadOnly()
        };
    }
}
