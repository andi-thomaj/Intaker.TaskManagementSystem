using System.Text.Json;
using Intaker.TaskManagementSystem.infrastructure.Helpers;

namespace Intaker.TaskManagementSystem.api
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ApiErrorResponse();

            switch (exception)
            {
                case FluentValidation.ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = "Validation failed";
                    response.Errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                    response.Details = "One or more validation errors occurred";
                    break;

                case KeyNotFoundException notFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = "Resource not found";
                    response.Details = notFoundException.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "Internal server error";
                    response.Details = exception.Message;
                    logger.LogError(exception, "An unhandled exception occurred");
                    break;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
