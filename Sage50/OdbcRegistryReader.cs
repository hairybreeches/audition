using System;
using System.Collections.Generic;
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
            IRegistryKey driversKey;

            if (!reader.TryOpenKey("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers", out driversKey))
            {
                throw new SageNotInstalledException();                                
            }

            return driversKey.GetValueNames();
        }
    }
}