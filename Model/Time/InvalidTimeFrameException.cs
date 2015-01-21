using System;
using System.Runtime.Serialization;

namespace Model.Time
{
    public class InvalidTimeFrameException : Exception
    {
        public InvalidTimeFrameException()
        {
        }

        public InvalidTimeFrameException(string message) : base(message)
        {
        }

        public InvalidTimeFrameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTimeFrameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
