using Microsoft.Win32;

namespace Native
{
    public class RegistryReader
    {
        public IRegistryKey OpenKey(string registryKey)
        {
            return  new AuditionRegistryKey(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(registryKey));
        }
    }
}