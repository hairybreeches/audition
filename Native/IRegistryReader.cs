namespace Native
{
    public interface IRegistryReader
    {
        bool TryOpenKey(string keyName, out IRegistryKey registryKey);
        bool TryGetKeyValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey);
    }
}