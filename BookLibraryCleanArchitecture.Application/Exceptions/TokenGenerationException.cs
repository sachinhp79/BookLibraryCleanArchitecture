using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Exceptions
{
    public class TokenGenerationException : Exception
    {
        public string ErrorCode { get; }
        public TokenGenerationException(string message, string errorCode, Exception? innerException = null) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
