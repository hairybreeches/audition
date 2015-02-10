using System;

namespace Model
{
    public class NoJournalsException : Exception
    {
        public NoJournalsException(string message)
            :base(message)
        {
        }
    }
}