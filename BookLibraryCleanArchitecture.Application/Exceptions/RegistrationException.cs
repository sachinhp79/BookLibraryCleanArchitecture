using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.Exceptions
{
    public class RegistrationException : Exception
    {
        public string ErrorCode { get; }
        public RegistrationException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
