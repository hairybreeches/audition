using System.Collections.Generic;
using Microsoft.Win32;

namespace Native
{
    public class AuditionRegistryKey : IRegistryKey
    {
        private readonly RegistryKey nativeKey;

        public AuditionRegistryKey(RegistryKey nativeKey)
        {
            this.nativeKey = nativeKey;            
        }

        public IEnumerable<string> GetValueNames()
        {
            return nativeKey.GetValueNames();
        }


        public bool TryGetStringValue(string name, out string value)
        {
            value = nativeKey.GetValue(name) as string;
            return value != null;
        }
    }
}