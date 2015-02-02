using System;

namespace Persistence
{
    public class InvalidPageNumberException : Exception
    {
        public InvalidPageNumberException(string message)
            :base(message)
        {            
        }
    }
}