using BookLibraryCleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Interfaces
{
    public interface IAuthenticationProcessor
    {
        string GenerateJwtToken(ApplicationUser user);
        string GenerateRefreshToken();
        void WriteTokenToCookie(string token, int expirationTimeInDays);
    }
}
