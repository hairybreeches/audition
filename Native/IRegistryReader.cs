using System.Collections.Generic;

namespace Native
{
    public interface IRegistryReader
    {
        bool TryGetStringValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey);
        bool TryGetValueNames(string location, out IEnumerable<string> valueNames);
    }

    public interface ILocalMachineRegistryReader : IRegistryReader
    {
        
    }

    public interface ICurrentUserRegistryReader : IRegistryReader
    {
        
    }
}