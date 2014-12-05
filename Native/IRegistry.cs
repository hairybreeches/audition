using System.Collections.Generic;

namespace Native
{
    public interface IRegistry
    {
        bool TryGetStringValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey);
        bool TryGetValueNames(string location, out IEnumerable<string> valueNames);
    }

    public interface ILocalMachineRegistry : IRegistry
    {
        
    }

    public interface ICurrentUserRegistry : IRegistry
    {
        
    }
}