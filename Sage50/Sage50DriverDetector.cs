using System;
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
            var sortedSageDrivers = sageDrivers.Select(CreateDriver)
                .Where(x => x != null)
                .OrderByDescending(x => x.Version);            
            return sortedSageDrivers.First();
        }

        private static Sage50Driver CreateDriver(string name)
        {
            try
            {
                return Sage50Driver.Create(name);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}