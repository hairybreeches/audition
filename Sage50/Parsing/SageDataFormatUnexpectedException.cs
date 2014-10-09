using System;

namespace Sage50.Parsing
{
    internal class SageDataFormatUnexpectedException : Exception
    {
        public SageDataFormatUnexpectedException(string message)
            :base(message)
        {            
        }
    }
}