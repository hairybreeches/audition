using System;

namespace Native
{
    public class RegistryKeyDoesNotExistException : Exception
    {
        public RegistryKeyDoesNotExistException(string registryKey)
            :base(String.Format("Could not open registry key: \"{0}\"", registryKey))
        {
            
        }
    }
}