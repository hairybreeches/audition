using System.Collections.Generic;

namespace UserData
{
    public interface IDemoDataSupplier
    {
        IEnumerable<string> GetDemoDataLocations();
    }
}