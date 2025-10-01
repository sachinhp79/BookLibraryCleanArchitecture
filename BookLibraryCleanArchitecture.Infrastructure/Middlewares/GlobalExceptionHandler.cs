using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Common.Dtos;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookLibraryCleanArchitecture.Infrastructure.Middlewares
{
    public sealed class GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        IEnumerable<IExceptionProblemDetailsMapper> exceptionProblemDetailsMappers) : IExceptionHandler
    {
        private readonly IEnumerable<IExceptionProblemDetailsMapper> _exceptionProblemDetailsMappers = exceptionProblemDetailsMappers;
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var correlationId = context.Items[MiddlewareConstants.CORRELATION_ID]?.ToString() ?? context.TraceIdentifier;
            context.Response.Headers[MiddlewareConstants.CORRELATION_ID_HEADER] = correlationId;

            var mapper = _exceptionProblemDetailsMappers.FirstOrDefault(m => m.CanHandle(exception));

            var problemDetailsFromMapper = mapper?.Map(exception, correlationId);

            await _problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problemDetailsFromMapper!,
                Exception = exception
            });
            return true;
        }
    }
}
