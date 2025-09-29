using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Common.Dtos;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Infrastructure.Mappers
{
    public class RegistrationExceptionMapper : IExceptionProblemDetailsMapper
    {
        public bool CanHandle(Exception exception) => exception is RegistrationException;

        public ProblemDetails Map(Exception exception, string correlationId)
        {
            var rex = exception as RegistrationException;

            return new ExtendedProblemDetails
            {
                Title = ValidationMessages.RegistrationErrorTitle,
                Status = StatusCodes.Status400BadRequest,
                Detail = rex?.Message ?? ValidationMessages.GenericRegistrationError,
                Message = rex?.Message ?? ValidationMessages.GenericRegistrationError,
                ErrorCode = rex?.ErrorCode
            };

            /* can do this to return status code based on ErrorCodes
            return new ProblemDetails
            {
                Title = "Registration Error",
                Detail = rex.Message,
                Status = rex.ErrorCode switch
                {
                    "REGISTRATION_FAILED" => StatusCodes.Status400BadRequest,
                    "ROLE_ASSIGNMENT_FAILED" => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                },
                Instance = correlationId
            }
            */
        }
    }
}
