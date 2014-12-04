﻿using System.Collections.Generic;
using Native;

namespace Sage50
{
    public class OdbcRegistryReader : IOdbcRegistryReader
    {
        private readonly RegistryReader reader;

        public OdbcRegistryReader(RegistryReader reader)
        {
            this.reader = reader;
        }

        public IEnumerable<string> Get32BitOdbcDrivers()
        {
            var driversKey = reader.OpenKey("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers");

            if (driversKey == null)
            {
                throw new SageNotInstalledException();
            }

            return driversKey.GetValueNames();
        }
    }
}