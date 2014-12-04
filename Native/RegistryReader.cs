using Microsoft.Win32;

namespace Native
{
    public class RegistryReader : IRegistryReader
    {
        public IRegistryKey OpenKey(string keyName)
        {
            IRegistryKey key;

            if (TryOpenKey(keyName, out key))
            {
                return key;                
            }

            throw new RegistryKeyDoesNotExistException(keyName);            
        }

        public bool TryOpenKey(string keyName, out IRegistryKey registryKey)
        {
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(keyName);

            if (key == null)
            {
                registryKey = null;
                return false;
            }

            registryKey = new AuditionRegistryKey(key);
            return true;
        }
    }
}