using BookLibraryCleanArchitecture.Application.Enums;
using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Common.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
            _logger.LogError(rex, $"Registration failed: {rex.ErrorCode} - {correlationId}.");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errorResponse = ExceptionHelper.CreateErrorResponse(
                MiddlewareConstants.RegistrationError,
                StatusCodes.Status400BadRequest,
                rex.ErrorCode,
                rex.Message,
                correlationId
            );

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Validation failed - {CorrelationId}.", correlationId);

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
        catch(AuthenticationException aex)
        {

            // ToDo: Create a Helper which returns Error Response for different scenarios
            _logger.LogError(aex, "Authentication failed - {CorrelationId}.", correlationId);
            if (aex.ErrorCode == ErrorInformation.INVALID_TOKEN.ToString())
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    title = "Authentication Error",
                    status = 401,
                    errorCode = aex.ErrorCode,
                    message = "Invalid token.",
                    correlationId
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            else
            {
                // Handle other authentication errors if needed
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    title = "Authentication Error",
                    status = 403,
                    errorCode = aex.ErrorCode,
                    message = aex.Message,
                    correlationId
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception - {CorrelationId}.", correlationId);

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
