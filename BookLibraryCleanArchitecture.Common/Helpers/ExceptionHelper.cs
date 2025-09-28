using BookLibraryCleanArchitecture.Common.Dtos;
using System.Net;

    namespace BookLibraryCleanArchitecture.Common.Helpers
    {
        public static class ExceptionHelper
        {
            /// <summary>
            /// Create an <see cref="ErrorResponseDto"/> using an int status code (keeps compatibility).
            /// Invalid/unknown integer codes fall back to <see cref="HttpStatusCode.InternalServerError"/>.
            /// </summary>
            public static ErrorResponseDto CreateErrorResponse(
                string? title,
                int statusCode,
                string? errorCode,
                string? message,
                string? correlationId)
            {
                var resolvedStatus = Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                    ? (HttpStatusCode)statusCode
                    : HttpStatusCode.InternalServerError;

                return CreateErrorResponse(title, resolvedStatus, errorCode, message, correlationId, null);
            }

            /// <summary>
            /// Create an <see cref="ErrorResponseDto"/> using a <see cref="HttpStatusCode"/>.
            /// Accepts optional validation errors.
            /// </summary>
            public static ErrorResponseDto CreateErrorResponse(
                string? title,
                HttpStatusCode statusCode,
                string? errorCode,
                string? message,
                string? correlationId,
                IEnumerable<ValidationErrorDto>? errors)
            {
                var safeTitle = string.IsNullOrWhiteSpace(title) ? "Error" : title!;
                var safeCorrelationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString("D") : correlationId!;

                return new ErrorResponseDto
                {
                    Title = safeTitle,
                    StatusCode = statusCode,
                    ErrorCode = errorCode,
                    Message = message,
                    Errors = errors,
                    CorrelationId = safeCorrelationId
                };
        }
    }
}
