using Microsoft.AspNetCore.Identity;

namespace BookLibraryCleanArchitecture.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Role { get; set; }
        public string? FullName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTimeInUtc { get; set; }

        public override string ToString() => $"{FullName} ({Email})";

        public static ApplicationUser Create(string firstName, string lastName, string email, string userName)
        {
            return new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = userName,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = Roles.User.ToString(),
            };
        }

    }
}
