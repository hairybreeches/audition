using Microsoft.Win32;

namespace Native
{
    public class RegistryReader : IRegistryReader
    {
        public IRegistryKey OpenKey(string keyName)
        {
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(keyName);

            if (key == null)
            {
                throw new RegistryKeyDoesNotExistException(keyName);
            }

            return  new AuditionRegistryKey(key);
        }
    }
}