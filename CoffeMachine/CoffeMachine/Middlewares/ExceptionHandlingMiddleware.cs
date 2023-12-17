using System.Net;
using System.Text.Json;

using CoffeeMachine.Dto;

namespace CoffeeMachine.Middlewares;

/// <summary>
/// A class that defines custom Middleware for exception handling
/// </summary>
public class ExceptionHandlingMiddleware
{
    /// <summary>
    /// Defining logging
    /// </summary>
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Defining RequestDelegate
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor of the class in which we implement custom error handling
    /// </summary>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Implementing an error handler
    /// </summary>
    /// <param name="httpContext"> Http context </param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message, ex);
        }
        catch (InvalidDataException ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.Unauthorized, ex.Message, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, ex.Message, ex);
        }
    }

    /// <summary>
    /// Method to which control is transferred after an error occurs
    /// </summary>
    /// <param name="context"> Http context </param>
    /// <param name="httpStatusCode"> Http Status Code </param>
    /// <param name="message"> Error message </param>
    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode httpStatusCode,
        string message, Exception ex)
    {
        _logger.LogError(ex, message);

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