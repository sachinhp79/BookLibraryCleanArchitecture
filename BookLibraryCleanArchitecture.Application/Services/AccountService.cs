using BookLibraryCleanArchitecture.Application.Constants;
using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Application.Enums;
using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Application.Interfaces;
using BookLibraryCleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookLibraryCleanArchitecture.Application.Services
{
    public class AccountService(
        IAuthenticationProcessor authenticationProcessor,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountService> logger,
        IOptions<JwtOptions> options,
        RoleManager<IdentityRole<Guid>> roleManager
    ) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IAuthenticationProcessor _authenticationProcessor = authenticationProcessor;
        private readonly IOptions<JwtOptions> _options = options;
        private readonly ILogger<AccountService> _logger = logger;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;        

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request)
        {
            var userExists = await _userManager.FindByNameAsync(request.UserName);
            
            if (userExists is not null)
                throw new RegistrationException("User already exists!", ErrorInformation.USER_ALREADY_EXISTS.ToString());

            var roleName = Roles.User.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!createRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", createRoleResult.Errors.Select(e => e.Description));
                    throw new RegistrationException($"Failed to create role '{roleName}': {errors}", ErrorInformation.ROLE_ASSIGNMENT_FAILED.ToString());
                }
            }

            var user = ApplicationUser.Create(request.FirstName, request.LastName, request.Email, request.Email);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new RegistrationException($"User Registration Failed. {errors}", ErrorInformation.REGISTRATION_FAILED.ToString());
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new RegistrationException($"Failed to assign role: {errors}", ErrorInformation.ROLE_ASSIGNMENT_FAILED.ToString());
            }

            return new RegisterResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? user.Email ?? string.Empty,
                Email = user.Email!,
            };
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto loginReuestDto)
        {

            var user = await _userManager.FindByNameAsync(loginReuestDto.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginReuestDto.Password))
            {
                throw new AuthenticationException("Invalid username or password.", ErrorInformation.INVALID_CREDENTIALS.ToString());
            }

            try
            {
                var accessToken = _authenticationProcessor.GenerateJwtToken(user);
                var refreshToken = _authenticationProcessor.GenerateRefreshToken();

                user.RefreshTokenExpiryTimeInUtc = DateTime.UtcNow.AddDays(_options.Value.RefreshTokenExpirationInDays);
                var result = await _userManager.UpdateAsync(user);
                if(!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new TokenGenerationException($"Failed to update user with refresh token. {errors}", ErrorInformation.TOKEN_GENERATION_FAILED.ToString());
                }

                // writing tokens to HttpOnly cookies
                _authenticationProcessor.WriteTokenToCookie(accessToken, ApplicationConstants.JwtAccessToken, _options.Value.ExpirationTimeInMinutes);
                _authenticationProcessor.WriteTokenToCookie(accessToken, ApplicationConstants.RefreshToken, _options.Value.RefreshTokenExpirationInDays);

                return new LoginResponseDto
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiresAtUtc = user.RefreshTokenExpiryTimeInUtc!.Value
                };
            }
            catch (TokenGenerationException ex)
            {
                _logger.LogError(ex, "An error occurred while generating tokens for user {UserId}", user.Id);
                throw new TokenGenerationException("Failed to generate tokens. Please try again.", ErrorInformation.TOKEN_GENERATION_FAILED.ToString(), ex.InnerException);
            }
            catch(Exception ex)
            {
                throw new AuthenticationException("Unexpected Error during logging.", ErrorInformation.LOGIN_FAILED.ToString(), ex.InnerException);
            }
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new TokenGenerationException("Refresh token is required.", ErrorInformation.INVALID_TOKEN.ToString());
            }

            var user = _userManager.Users.FirstOrDefault(u => string.Equals(u.RefreshToken, refreshToken));
            if (user is null || user.RefreshTokenExpiryTimeInUtc <= DateTime.UtcNow)
            {
                throw new TokenGenerationException("Invalid or expired refresh token.", ErrorInformation.INVALID_TOKEN.ToString());
            }
             
            var newAccessToken = _authenticationProcessor.GenerateJwtToken(user);
            var newRefreshToken = _authenticationProcessor.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTimeInUtc = DateTime.UtcNow.AddDays(_options.Value.RefreshTokenExpirationInDays);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new TokenGenerationException($"Failed to update user with new refresh token. {errors}", ErrorInformation.TOKEN_GENERATION_FAILED.ToString());
            }
            // writing tokens to HttpOnly cookies
            _authenticationProcessor.WriteTokenToCookie(newAccessToken, ApplicationConstants.JwtAccessToken, _options.Value.ExpirationTimeInMinutes);
            _authenticationProcessor.WriteTokenToCookie(newRefreshToken, ApplicationConstants.RefreshToken, _options.Value.RefreshTokenExpirationInDays);

            return new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAtUtc = user.RefreshTokenExpiryTimeInUtc!.Value
            };
        }

        //public async Task LogoutUserAsync()
        //{
        //    // Invalidate the refresh token in the database
        //    var user = await _userManager.GetUserAsync(new ClaimsPrincipal());
        //    if (user != null)
        //    {
        //        user.RefreshToken = null;
        //        user.RefreshTokenExpiryTimeInUtc = null;
        //        await _userManager.UpdateAsync(user);
        //    }
        //}
    }
}
