using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Options;

namespace BookLibraryCleanArchitecture.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string BuildToken(ApplicationUser user, IOptions<JwtOptions> options);
    }
}
