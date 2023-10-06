namespace CoffeMachine.Middlewares
{
    using System.Net;
    using System.Text.Json;

    using CoffeMachine.Dto;

    using Microsoft.AspNetCore.Mvc.TagHelpers;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode httpStatusCode,
            string message)
        {
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