namespace Native
{
    public interface IRegistryReader
    {
        IRegistryKey OpenKey(string keyName);
    }
}