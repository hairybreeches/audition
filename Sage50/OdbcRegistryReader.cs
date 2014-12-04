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

            try
            {
                driversKey = reader.OpenKey("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers");
            }
            catch (RegistryKeyDoesNotExistException)
            {
                throw new SageNotInstalledException();                
            }

            return driversKey.GetValueNames();
        }
    }
}