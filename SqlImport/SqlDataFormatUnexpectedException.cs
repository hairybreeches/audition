using System;

namespace SqlImport
{
    public class SqlDataFormatUnexpectedException : Exception
    {
        public SqlDataFormatUnexpectedException(string message)
            :base(message)
        {            
        } 
        
        public SqlDataFormatUnexpectedException(string message, Exception inner)
            :base(message, inner)
        {            
        }
    }
}