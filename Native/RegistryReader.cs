using Microsoft.Win32;

namespace Native
{
    public class RegistryReader : IRegistryReader
    {
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

        public bool TryGetKeyValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey)
        {
            IRegistryKey key;
            if (!TryOpenKey(licenceKeyLocation, out key))
            {
                licenceKey = null;
                return false;
            }

            return key.TryGetStringValue(licenceKeyName, out licenceKey);
        }
    }
}