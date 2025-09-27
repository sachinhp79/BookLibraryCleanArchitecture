namespace BookLibraryCleanArchitecture.Application.Dtos
{
    public class JwtOptions
    {
        public const string JwtOptionsSection = "JwtOptions";
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SecretKey { get; set; }
        public required int ExpirationTimeInMinutes { get; set; }
    }
}
