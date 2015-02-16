using System;

namespace Model
{
    public class NoTransactionsException : Exception
    {
        public NoTransactionsException(string message)
            :base(message)
        {
        }
    }
}