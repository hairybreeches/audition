using Microsoft.Win32;

namespace Native
{
    public class RegistryReader
    {
        public RegistryKey OpenKey(string registryKey)
        {
            return  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(registryKey);
        }
    }
}