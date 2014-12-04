using Microsoft.Win32;

namespace Native
{
    public class RegistryReader : IRegistryReader
    {
        public IRegistryKey OpenKey(string registryKey)
        {
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(registryKey);

            if (key == null)
            {
                throw new RegistryKeyDoesNotExistException(registryKey);
            }

            return  new AuditionRegistryKey(key);
        }
    }
}