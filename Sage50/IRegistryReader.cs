using System.Collections.Generic;

namespace Sage50
{
    public interface IRegistryReader
    {
        IEnumerable<string> Get32BitOdbcDrivers();
    }
}