using System.IO;
using Native;
using UserData;

namespace Sage50
{
    public class Sage50DataDirectoryStorage : DataLocationStorage
    {
        public Sage50DataDirectoryStorage(IUserDetailsStorage userDetailsStorage, SageDemoDirectorySupplier demoDirectorySupplier)
            :base(userDetailsStorage, demoDirectorySupplier, details => details.Sage50DataLocations, details => details.AddSage50DataLocation)
        {
        }
    }
}
