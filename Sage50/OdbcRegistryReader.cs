using System;
using System.Collections.Generic;
using System.Linq;
using Native;

namespace Sage50
{
    public class OdbcRegistryReader
    {
        private readonly IRegistryReader reader;

        public OdbcRegistryReader(IRegistryReader reader)
        {
            this.reader = reader;
        }

        public IEnumerable<string> Get32BitOdbcDrivers()
        {
            IEnumerable<string> driverNames;
            if (!TryGetValueNames("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers", out driverNames))
            {
                throw new SageNotInstalledException();
            }

            return driverNames;
        }

        public bool TryGetValueNames(string location, out IEnumerable<string> valueNames)
        {
            IRegistryKey key;

            if (!reader.TryOpenKey(location, out key))
            {
                valueNames = Enumerable.Empty<string>();
                return false;
            }


            valueNames = key.GetValueNames();
            return true;
        }
    }
}