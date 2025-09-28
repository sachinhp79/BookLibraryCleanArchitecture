using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Dtos
{
    public class LoginRequestDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
