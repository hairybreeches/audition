using System.Globalization;
using System.Linq;

namespace Sage50
{
    public class Sage50DriverDetector
    {
        private readonly IRegistryReader registryReader;

        public Sage50DriverDetector(IRegistryReader registryReader)
        {
            this.registryReader = registryReader;
        }

        public Sage50Driver FindBestDriver()
        {
            var driverNames = registryReader.Get32BitOdbcDrivers();
            var sageDrivers = driverNames.Where(x => x.StartsWith("Sage Line 50", true, CultureInfo.CurrentCulture));
            var sortedSageDrivers = sageDrivers.Select(Sage50Driver.Create).OrderByDescending(x => x.Version);            
            return sortedSageDrivers.First();
        }
    }
}