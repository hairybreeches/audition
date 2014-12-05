using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Native
{
    public class Registry : ILocalMachineRegistry, ICurrentUserRegistry
    {
        private readonly RegistryHive hive;

        public Registry(RegistryHive hive)
        {
            this.hive = hive;
        }

        private bool TryOpenKey(string keyName, out RegistryKey registryKey)
        {            
            registryKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry32)
                .OpenSubKey(keyName);

            return registryKey != null;
        }

        public bool TryGetStringValue(string location, string keyName, out string keyValue)
        {
            RegistryKey key;
            if (!TryOpenKey(location, out key))
            {
                keyValue = null;
                return false;
            }

            return TryGetStringValue(key, keyName, out keyValue);
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