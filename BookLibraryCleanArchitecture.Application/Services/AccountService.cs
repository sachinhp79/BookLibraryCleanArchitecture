using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Application.Enums;
using BookLibraryCleanArchitecture.Application.Exceptions;
using BookLibraryCleanArchitecture.Application.Interfaces;
using BookLibraryCleanArchitecture.Domain.Entities;
using BookLibraryCleanArchitecture.Domain.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthenticationProcessor _authenticationProcessor;

        public AccountService(IAuthenticationProcessor authenticationProcessor, UserManager<ApplicationUser> userManager) 
        {
            _authenticationProcessor = authenticationProcessor;
            this.userManager = userManager;
        }   

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request)
        {
            var userExists = await userManager.FindByEmailAsync(request.Email);
            
            if (userExists is not null)
                throw new RegistrationException("User already exists!", ErrorInformation.USER_ALREADY_EXISTS.ToString());

            var user = ApplicationUser.Create(request.FirstName, request.LastName, request.Email, request.Email);

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new RegistrationException($"User Registration Failed. {errors}", ErrorInformation.REGISTRATION_FAILED.ToString());
            }

            var roleResult = await userManager.AddToRoleAsync(user, Role.User.ToString());
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new RegistrationException($"Failed to assign role: {errors}", ErrorInformation.ROLE_ASSIGNMENT_FAILED.ToString());
            }

            return new RegisterResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email!,
                //AccessToken = accessToken,
                //RefreshToken = refreshToken,
                //RefreshTokenExpiresAtUtc = user.RefreshTokenExpiryTimeInUtc!.Value
            };
        }

    }
}
