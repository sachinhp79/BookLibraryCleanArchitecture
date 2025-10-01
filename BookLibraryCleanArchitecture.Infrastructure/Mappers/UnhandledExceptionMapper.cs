using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BookLibraryCleanArchitecture.Infrastructure.Mappers
{
    public sealed class DefaultExceptionProblemDetailsMapper : IExceptionProblemDetailsMapper
    {
        private readonly IWebHostEnvironment _env;
        public DefaultExceptionProblemDetailsMapper(IWebHostEnvironment env) => _env = env;

        public bool CanHandle(Exception exception) => true; // fallback

        public ProblemDetails Map(Exception exception, string correlationId)
        {
            var pd = new ProblemDetails
            {
                Title = ValidationMessages.UnhandledExceptionErrorTitle,
                Status = StatusCodes.Status500InternalServerError,
                Detail = _env.IsDevelopment() ? exception.Message : ValidationMessages.GenericUnhandledExceptionError,
                Type = exception.GetType().FullName
            };
            pd.Extensions[MiddlewareConstants.CORRELATION_ID] = correlationId;
            if (_env.IsDevelopment()) pd.Extensions["stackTrace"] = exception.StackTrace;
            return pd;
        }
    }
}
