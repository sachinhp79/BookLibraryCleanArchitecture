using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookLibraryCleanArchitecture.Infrastructure.Services
{
    public class BookLibraryProblemDetailsService : IProblemDetailsService
    {
        private readonly ILogger<BookLibraryProblemDetailsService> _logger;

        public BookLibraryProblemDetailsService(ILogger<BookLibraryProblemDetailsService> logger)
        {
            _logger = logger;
        }

        public async ValueTask WriteAsync(ProblemDetailsContext context)
        {
            var httpContext = context.HttpContext;
            var correlationId = httpContext.Items[MiddlewareConstants.CORRELATION_ID] ?? httpContext.TraceIdentifier;
            var problemDetails = context.ProblemDetails;

            _ = problemDetails is ExtendedProblemDetails extendedProblemDetails
                ? extendedProblemDetails.CorrelationId = correlationId?.ToString()
                : problemDetails.Extensions["correlationId"] = correlationId?.ToString();

            _logger.LogError(context.Exception, "Error occurred: {Title} - {CorrelationId}", problemDetails.Title, correlationId);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails);

        }
    }
}
