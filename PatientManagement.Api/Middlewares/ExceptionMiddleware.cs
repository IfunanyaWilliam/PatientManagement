
namespace PatientManagement.Api.Middlewares
{
    using System.Text.Json;
    using PatientManagement.Api.Results;
    using Application.Utilities;

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

            var errorResponse = new ExceptionResult();

            switch (ex)
            {
                case CustomException customException:
                    context.Response.StatusCode = customException.StatusCode;
                    errorResponse.StatusCode = customException.StatusCode;
                    errorResponse.Message = customException.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.Message = "An internal server error occurred.";
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
