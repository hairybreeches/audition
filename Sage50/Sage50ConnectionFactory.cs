using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using Model;

namespace Sage50
{
    public class Sage50ConnectionFactory : ISage50ConnectionFactory
    {
        private readonly Sage50DriverDetector driverDetector;

        public Sage50ConnectionFactory(Sage50DriverDetector driverDetector)
        {
            this.driverDetector = driverDetector;
        }

        public DbConnection OpenConnection(Sage50LoginDetails loginDetails)
        {
            var drivers = driverDetector.FindSageDrivers();
            foreach (var driver in drivers)
            {
                try
                {
                    return OpenConnection(loginDetails, driver);
                }
                catch (CannotOpenDataDirectoryException)
                {
                }
            }

            throw new IncorrectLoginDetailsException("Could not open the specified folder with available versions of Sage 50.\n" +
                                                     "Check that the folder is a Sage 50 data directory and that the correct version of Sage is installed.\n" +
                                                     "The data directory can be found by logging in to Sage and clicking help->about from the menu.\n" +            
                                                     "Available versions of Sage 50: " + String.Join(", ", drivers.Select(x=>x.FriendlyName)));            
        }

        private DbConnection OpenConnection(Sage50LoginDetails loginDetails, Sage50Driver sage50Driver)
        {
            var connectionString = CreateConnectionString(loginDetails, sage50Driver);
            var conn = new OdbcConnection(connectionString);
            OpenConnection(conn);
            return conn;
        }

        private static void OpenConnection(IDbConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (OdbcException e)
            {
                var error = e.Errors[0];
                if (error.SQLState == "08001")
                {
                    throw new CannotOpenDataDirectoryException();
                }
                if (error.SQLState == "28000")
                {
                    throw new IncorrectLoginDetailsException("Incorrect username or password");
                }

                throw;
            }
        }

        private static string CreateConnectionString(Sage50LoginDetails loginDetails, Sage50Driver driver)
        {
            var builder = new OdbcConnectionStringBuilder
            {
                Driver = driver.Name
            };

            builder["uid"] = loginDetails.Username;
            builder["dir"] = loginDetails.DataDirectory;

            if (!string.IsNullOrEmpty(loginDetails.Password))
            {
                builder["pwd"] = loginDetails.Password;
            }

            var connectionString = builder.ConnectionString;
            return connectionString;
        }
    }

    internal class CannotOpenDataDirectoryException : Exception
    {
    }
}