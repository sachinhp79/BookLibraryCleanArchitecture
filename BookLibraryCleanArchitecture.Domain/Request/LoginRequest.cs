﻿namespace BookLibraryCleanArchitecture.Domain.Request
{
    public class LoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }

    }
}
