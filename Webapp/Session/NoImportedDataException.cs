using System;

namespace Webapp.Session
{
    public class NoImportedDataException : Exception      
    {
        public NoImportedDataException()
            :base("No data has been imported")
        {
        }
    }
}