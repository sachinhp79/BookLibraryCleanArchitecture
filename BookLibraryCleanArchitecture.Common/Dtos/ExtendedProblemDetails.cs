using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Common.Dtos
{
    public class ExtendedProblemDetails : ProblemDetails
    {
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
        public string? CorrelationId { get; set; }
        public IEnumerable<ValidationErrorDto>? Errors { get; set; }
    }
}
