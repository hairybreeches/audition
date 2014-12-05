using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Native
{
    public class RegistryReader : IRegistryReader
    {
        private static bool TryOpenKey(string keyName, out RegistryKey registryKey)
        {
            registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(keyName);

            return registryKey != null;
        }

        public bool TryGetKeyValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey)
        {
            RegistryKey key;
            if (!TryOpenKey(licenceKeyLocation, out key))
            {
                licenceKey = null;
                return false;
            }

            return TryGetStringValue(key, licenceKeyName, out licenceKey);
        }

        public bool TryGetValueNames(string location, out IEnumerable<string> valueNames)
        {
            RegistryKey key;

            if (!TryOpenKey(location, out key))
            {
                valueNames = Enumerable.Empty<string>();
                return false;
            }


            valueNames = key.GetValueNames();
            return true;
        }

        private static bool TryGetStringValue(RegistryKey nativeKey, string name, out string value)
        {
            value = nativeKey.GetValue(name) as string;
            return value != null;
        }
    }
}