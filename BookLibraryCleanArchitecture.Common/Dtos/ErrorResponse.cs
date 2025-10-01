using BookLibraryCleanArchitecture.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;

namespace BookLibraryCleanArchitecture.Common.Dtos
{
    public sealed record ErrorResponseDto
    {
        [JsonPropertyName("title")]
        public string Title { get; init; } = string.Empty;

        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; init; }

        [JsonPropertyName("message")]
        public string? Message { get; init; }

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; init; }

        [JsonPropertyName("errors")]
        public IEnumerable<ValidationErrorDto>? Errors { get; init; }

        [JsonPropertyName(MiddlewareConstants.CORRELATION_ID)]
        public string CorrelationId { get; init; } = string.Empty;
    }
}
