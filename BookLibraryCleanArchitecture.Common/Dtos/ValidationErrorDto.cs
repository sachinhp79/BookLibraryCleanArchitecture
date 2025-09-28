using System.Text.Json.Serialization;

namespace BookLibraryCleanArchitecture.Common.Dtos
{
    public sealed record ValidationErrorDto
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("message")]
        public required string Message { get; set; }
    }
}
