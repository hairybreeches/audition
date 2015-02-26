using System;

namespace ExcelExport
{
    public class ExcelInteropException : Exception
    {
        public ExcelInteropException(string message)
            : base(message)
        {
        }
    }
}