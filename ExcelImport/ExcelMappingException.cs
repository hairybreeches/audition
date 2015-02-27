using System;

namespace ExcelImport
{
    public class ExcelMappingException : Exception
    {
        public ExcelMappingException(string message)
            :base(message)
        {
        }
    }
}