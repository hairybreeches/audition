using System;

namespace Model
{
    public class IncorrectLoginDetailsException : Exception
    {
        public IncorrectLoginDetailsException(string message)
            :base(message)
        {            
        }
    }
}