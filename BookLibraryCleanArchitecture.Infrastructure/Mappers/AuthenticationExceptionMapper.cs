using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Common.Dtos;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Infrastructure.Mappers
{
    public class AuthenticationExceptionMapper : IExceptionProblemDetailsMapper
    {
        public bool CanHandle(Exception exception) => exception is AuthenticationException;

        public ProblemDetails Map(Exception exception, string correlationId)
        {
            var aex = (AuthenticationException)exception;

            return new ExtendedProblemDetails
            {
                Title = ValidationMessages.AuthenticationErrorTitle,
                Status = StatusCodes.Status401Unauthorized,
                Detail = aex?.Message ?? ValidationMessages.GenericAutheticationError,
                Message = aex?.Message ?? ValidationMessages.GenericAutheticationError,
                ErrorCode = aex?.ErrorCode,
                CorrelationId = correlationId
            };
        }
    }

}
