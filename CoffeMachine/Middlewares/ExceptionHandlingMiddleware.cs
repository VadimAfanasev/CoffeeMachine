using System.Net;
using System.Text.Json;
using CoffeeMachine.Dto;

namespace CoffeeMachine.Middlewares
{

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidDataException ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode httpStatusCode,
            string message)
        {
            _logger.LogError(message);

            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            var errorDto = new ErrorDto
            {
                Message = message,
                StatusCode = (int)httpStatusCode
            };

            var result = JsonSerializer.Serialize(errorDto);
            await response.WriteAsJsonAsync(result);
        }
    }
}