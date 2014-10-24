using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Win32;

namespace Sage50
{
    public class Sage50DriverDetector
    {
        public Sage50Driver FindBestDriver()
        {
            var driverNames = DriverNames();
            var sageDrivers = driverNames.Where(x => x.StartsWith("Sage Line 50", true, CultureInfo.CurrentCulture));
            var sortedSageDrivers = sageDrivers.Select(Sage50Driver.Create).OrderByDescending(x => x.Version);            
            return sortedSageDrivers.First();
        }

        private static IEnumerable<string> DriverNames()
        {
            var driversKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers");

            if (driversKey == null)
            {
                throw new SageNotInstalledException();
            }

            return driversKey.GetValueNames();
        }
    }
}