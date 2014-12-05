using System;
using System.Collections.Generic;
using System.Linq;
using Native;

namespace Sage50
{
    public class OdbcRegistryReader
    {
        private readonly IRegistry reader;

        public OdbcRegistryReader(ILocalMachineRegistry reader)
        {
            this.reader = reader;
        }

        public IEnumerable<string> Get32BitOdbcDrivers()
        {
            IEnumerable<string> driverNames;
            if (!reader.TryGetValueNames("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers", out driverNames))
            {
                throw new SageNotInstalledException();
            }

            return driverNames;
        }
    }
}