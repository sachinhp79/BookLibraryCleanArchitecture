using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Application.Interfaces;
using BookLibraryCleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace BookLibraryCleanArchitecture.Infrastructure.Processors
{
    public class AuthenticationProcessor : IAuthenticationProcessor
    {
        private readonly IOptions<JwtOptions> _options;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly HttpContextAccessor httpContextAccessor;

        public AuthenticationProcessor(IOptions<JwtOptions> options, ITokenGenerator tokenGenerator, HttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _tokenGenerator = tokenGenerator;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            return _tokenGenerator.BuildToken(user, _options);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public void WriteTokenToCookie(string token, int expirationTimeInDays)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(expirationTimeInDays),
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            };

            httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
