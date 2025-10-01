using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BookLibraryCleanArchitecture.Infrastructure.Mappers
{
    public class ValidationExceptionMapper : IExceptionProblemDetailsMapper
    {
        public bool CanHandle(Exception exception) => exception is ValidationException;

        public ProblemDetails Map(Exception exception, string correlationId)
        {
            var vex = (ValidationException)exception;

            Dictionary<string, string[]> errors = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var details = new ValidationProblemDetails(errors)
            {
                Title = ValidationMessages.ValidationErrorTitle,
                Status = StatusCodes.Status400BadRequest
            };

            details.Extensions[MiddlewareConstants.CORRELATION_ID] = correlationId;
            return details;
        }
    }

}
