using System.Collections.Generic;
using Microsoft.Win32;

namespace Sage50
{
    public class RegistryReader : IRegistryReader
    {
        public IEnumerable<string> Get32BitOdbcDrivers()
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