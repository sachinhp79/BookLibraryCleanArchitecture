using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Application.Interfaces;
using BookLibraryCleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookLibraryCleanArchitecture.Infrastructure.Processors
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string secret) => new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
        private SigningCredentials GetSigningCredentials(string secret)
        {
            return new SigningCredentials(key: GetSymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        public string BuildToken(ApplicationUser user, IOptions<JwtOptions> options)
        {
            var jwtOptions = options.Value;
            var signingCredentials = GetSigningCredentials(jwtOptions.SecretKey);

            var jwtToken = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: BuildClaims(user),
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationTimeInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private IEnumerable<Claim> BuildClaims(ApplicationUser user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email)) throw new ArgumentException("User must have an email.", nameof(user));

            var fullName = !string.IsNullOrWhiteSpace(user.FullName)
                ? user.FullName!
                : $"{user.FirstName} {user.LastName}".Trim();

            yield return new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString());
            yield return new Claim(JwtRegisteredClaimNames.Email, user.Email!);
            yield return new Claim(JwtRegisteredClaimNames.Name, string.IsNullOrWhiteSpace(fullName) ? string.Empty : fullName);
            yield return new Claim(ClaimTypes.Role, user.Role.ToString());
        }
    }
}