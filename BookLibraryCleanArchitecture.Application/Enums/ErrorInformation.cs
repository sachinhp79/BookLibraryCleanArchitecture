using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Enums
{
    public enum ErrorInformation
    {
        USER_ALREADY_EXISTS = 1001,
        REGISTRATION_FAILED = 1002,
        ROLE_ASSIGNMENT_FAILED = 1003,
        INVALID_CREDENTIALS = 1004,
        TOKEN_GENERATION_FAILED = 1005,
        LOGIN_FAILED = 1006,
        INVALID_TOKEN = 1007,
        //InvalidCredentials = 1002,
        //UserNotFound = 1003,
        //WeakPassword = 1004,
        //UnauthorizedAccess = 1005,
        //TokenExpired = 1006,
        //InvalidToken = 1007,
        //RegistrationFailed = 1008,
        //LoginFailed = 1009
    }
}
