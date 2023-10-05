using System.Net;
using System.Text.Json;
using CoffeMachine.Dto;

namespace CoffeMachine.Middlewares
{
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, "Invalid data");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode httpStatusCode, string message)
        {
            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            var errorDto = new ErrorDto()
            {
                Message = message,
                StatusCode = (int)httpStatusCode
            };

            var result = JsonSerializer.Serialize(errorDto);
            await response.WriteAsJsonAsync(result);
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    try
        //    {
        //        await _next(context);
        //    }
        //    catch (Exception ex)
        //    {
        //        await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
        //    }
        //}

        //private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        //{
        //    context.Response.ContentType = "application/json";
        //    int statusCode = (int)HttpStatusCode.InternalServerError;
        //    var result = JsonConvert.SerializeObject(new
        //    {
        //        StatusCode = statusCode,
        //        ErrorMessage = exception.Message
        //    });
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = statusCode;
        //    return context.Response.WriteAsync(result);
        //}
    }
}
