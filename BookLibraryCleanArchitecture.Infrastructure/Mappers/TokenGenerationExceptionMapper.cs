using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Common.Constants;
using BookLibraryCleanArchitecture.Common.Dtos;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Infrastructure.Mappers
{
    public class TokenGenerationExceptionMapper : IExceptionProblemDetailsMapper
    {
        public bool CanHandle(Exception exception)
        {
            return exception is TokenGenerationException;
        }

        public ProblemDetails Map(Exception exception, string correlationId)
        {
            var tex = exception as TokenGenerationException;

            return new ExtendedProblemDetails
            {
                Title = ValidationMessages.TokenGenerationErrorTitle,
                Status = StatusCodes.Status500InternalServerError,
                Detail = tex?.Message ?? ValidationMessages.GenericTokenGenerationError,
                Message = tex?.Message ?? ValidationMessages.GenericTokenGenerationError,
                ErrorCode = tex?.ErrorCode
            };
        }
    }
}
