using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Native.RegistryAccess
{
    public class Registry : ILocalMachineRegistry, ICurrentUserRegistry, IDisposable
    {
       
        private readonly RegistryKey baseKey;

        public Registry(RegistryHive hive)
        {         
            baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry32);
        }

        private bool TryOpenKey(string keyName, out RegistryKey registryKey)
        {            
            registryKey = baseKey
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

        public void CreateLocation(string location)
        {
            CreateLocationInner(location);
        }

        private RegistryKey CreateLocationInner(string location)
        {
            return baseKey.CreateSubKey(location);
        }

        public void WriteValue(string location, string name, object value)
        {
            var key = CreateLocationInner(location);
            key.SetValue(name, value);
        }

        public void Dispose()
        {
            baseKey.Dispose();
        }

        public void DeleteLocation(string location)
        {
            baseKey.DeleteSubKeyTree(location);
        }
    }
}