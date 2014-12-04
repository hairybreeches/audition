using System.Collections.Generic;

namespace Native
{
    public interface IRegistryKey
    {
        IEnumerable<string> GetValueNames();
    }
}