using System;

namespace Sage50.Parsing
{
    public class SageDataFormatUnexpectedException : Exception
    {
        public SageDataFormatUnexpectedException(string message)
            :base(message)
        {            
        } 
        
        public SageDataFormatUnexpectedException(string message, Exception inner)
            :base(message, inner)
        {            
        }
    }
}