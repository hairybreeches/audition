namespace Native
{
    public interface IRegistryReader
    {
        bool TryOpenKey(string keyName, out IRegistryKey registryKey);
    }
}