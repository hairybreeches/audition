using System.Collections.Generic;

namespace Native
{
    public interface IRegistry
    {
        bool TryGetStringValue(string location, string keyName, out string keyValue);
        bool TryGetValueNames(string location, out IEnumerable<string> valueNames);
    }

    public interface ILocalMachineRegistry : IRegistry
    {
        
    }

    public interface ICurrentUserRegistry : IRegistry
    {
        
    }
}