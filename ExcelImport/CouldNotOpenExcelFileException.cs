using System;

namespace ExcelImport
{
    internal class CouldNotOpenExcelFileException : Exception
    {
        public CouldNotOpenExcelFileException(string message)
            :base(message)
        {            
        }
    }
}