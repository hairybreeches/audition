using System.Collections.Generic;

namespace Native.RegistryAccess
{
    public interface IRegistry
    {
        bool TryGetStringValue(string location, string keyName, out string keyValue);
        bool TryGetValueNames(string location, out IEnumerable<string> valueNames);
    }

    //although we will have write access to the Current User registry hive, since we won't be admin it's unlikely we will do Local Machine
    public interface ILocalMachineRegistry : IRegistry
    {
        
    }

    public interface ICurrentUserRegistry : IRegistry
    {
        void WriteValue(string location, string name, object value);
    }
}