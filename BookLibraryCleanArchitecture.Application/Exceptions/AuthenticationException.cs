using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Exceptions
{
    public class AuthenticationException : Exception
    {
        public string ErrorCode { get; }
        public AuthenticationException(string message, string errorCode, Exception? innerException = null) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
