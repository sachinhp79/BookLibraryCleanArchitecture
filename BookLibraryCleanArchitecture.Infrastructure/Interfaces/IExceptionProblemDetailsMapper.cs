using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Infrastructure.Interfaces
{
    public interface IExceptionProblemDetailsMapper
    {
        bool CanHandle(Exception exception);
        ProblemDetails Map(Exception exception, string correlationId);
    }
}
