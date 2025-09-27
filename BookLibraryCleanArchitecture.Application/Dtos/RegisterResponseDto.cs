using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Dtos
{
    public class RegisterResponseDto
    {
        // Unique identifier of the user (matches user.Id)
        public Guid UserId { get; init; }

        // Username of the registered user
        public string UserName { get; init; } = string.Empty;

        // Email of the registered user
        public string Email { get; init; } = string.Empty;

        // Access token issued at registration/login
        public string AccessToken { get; init; } = string.Empty;

        // Refresh token issued alongside the access token
        public string RefreshToken { get; init; } = string.Empty;

        // UTC expiry timestamp for the refresh token
        public DateTime RefreshTokenExpiresAtUtc { get; init; }

        // Parameterless constructor for serializers / framework usage
        public RegisterResponseDto() { }

        // Convenience constructor to create a fully populated DTO
        public RegisterResponseDto(
            Guid userId,
            string userName,
            string email,
            string accessToken,
            string refreshToken,
            DateTime refreshTokenExpiresAtUtc)
        {
            UserId = userId;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc;
        }
    }
}
