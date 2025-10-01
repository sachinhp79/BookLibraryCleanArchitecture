namespace BookLibraryCleanArchitecture.Application.Dtos
{
    public sealed record JwtOptions
    {
        public const string JwtOptionsSection = "JwtOptions";
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Secret { get; set; }
        public required int ExpirationTimeInMinutes { get; set; }
        public required int RefreshTokenExpirationInDays { get; set; }
    }
}
