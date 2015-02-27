using System;

namespace Sage50
{
    public class IncorrectSage50CredentialsException : Exception
    {
        public IncorrectSage50CredentialsException(string message)
            :base(message)
        {            
        }
    }
}