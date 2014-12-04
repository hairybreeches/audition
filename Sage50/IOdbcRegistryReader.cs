using System.Collections.Generic;

namespace Sage50
{
    public interface IOdbcRegistryReader
    {
        IEnumerable<string> Get32BitOdbcDrivers();
    }
}