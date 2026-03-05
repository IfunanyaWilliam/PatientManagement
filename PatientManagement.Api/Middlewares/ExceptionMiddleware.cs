
namespace PatientManagement.Api.Middlewares
{
    using Application.Utilities;
    using PatientManagement.Api.Results;
    using System.Text.Json;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError("Response already started, skipping exception handling.");
                return;
            }

            _logger.LogError(ex, "An exception occurred while processing the request.");

            // Set status only if response hasn't started
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            switch (ex)
            {
                case CustomException customException:
                    context.Response.StatusCode = customException.StatusCode;

                    var response = BaseResponse<object>.Fail
                    (
                        message: "Request processing failed.",
                         error: customException.Message,
                        responseCode: customException.ResponseCode
                    );

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    var generalResponse = BaseResponse<object>.Fail
                    (
                        message: "Request processing failed.",
                         error: "An internal server error occurred.",
                        responseCode: StatusCodes.Status500InternalServerError
                    );

                    await context.Response.WriteAsync(JsonSerializer.Serialize(generalResponse));
                    break;
            }
        }
    }
}
