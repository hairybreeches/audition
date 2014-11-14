using System;
using System.Runtime.Serialization;

namespace Model.Accounting
{
    public class InvalidJournalException : Exception
    {
        public InvalidJournalException()
        {
        }

        public InvalidJournalException(string message) : base(message)
        {
        }

        public InvalidJournalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidJournalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}