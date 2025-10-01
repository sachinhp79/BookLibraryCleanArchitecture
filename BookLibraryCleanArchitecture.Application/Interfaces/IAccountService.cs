using BookLibraryCleanArchitecture.Application.Dtos;

namespace BookLibraryCleanArchitecture.Application.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginUserAsync(LoginRequestDto loginReuestDto);
    }
}
