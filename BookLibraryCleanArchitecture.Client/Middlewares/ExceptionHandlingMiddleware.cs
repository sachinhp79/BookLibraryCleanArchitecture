using BookLibraryCleanArchitecture.Application.Exceptions;
using FluentValidation;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string correlationId = context.Items["CorrelationId"]?.ToString() ?? context.TraceIdentifier;

        try
        {
            await _next(context);
        }
        catch (RegistrationException rex)
        {
            _logger.LogWarning(rex, "Registration failed: {ErrorCode} - {CorrelationId}", rex.ErrorCode, correlationId);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                title = "Registration Error",
                status = 400,
                errorCode = rex.ErrorCode,
                //field = rex.Field,
                message = rex.Message,
                correlationId
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Validation failed - {CorrelationId}", correlationId);

            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                title = "Validation Error",
                status = 422,
                errors = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                correlationId
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception - {CorrelationId}", correlationId);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                title = "Server Error",
                status = 500,
                message = "An unexpected error occurred. Please contact support.",
                correlationId
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
