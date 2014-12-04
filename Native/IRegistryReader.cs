namespace Native
{
    public interface IRegistryReader
    {
        IRegistryKey OpenKey(string keyName);
        bool TryOpenKey(string keyName, out IRegistryKey registryKey);
    }
}