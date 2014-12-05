using System.Collections.Generic;

namespace Native
{
    public interface IRegistryReader
    {
        bool TryOpenKey(string keyName, out IRegistryKey registryKey);
        bool TryGetKeyValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey);
        bool TryGetValueNames(string location, out IEnumerable<string> valueNames);
    }
}