using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using Model;
using Native;
using Native.Disk;

namespace Sage50
{
    public class Sage50ConnectionFactory : ISage50ConnectionFactory
    {
        private readonly Sage50DriverDetector driverDetector;
        private readonly IFileSystem fileSystem;

        public Sage50ConnectionFactory(Sage50DriverDetector driverDetector, IFileSystem fileSystem)
        {
            this.driverDetector = driverDetector;
            this.fileSystem = fileSystem;
        }

        public DbConnection OpenConnection(Sage50ImportDetails importDetails)
        {
            var drivers = driverDetector.FindSageDrivers();
            var folder = GetFolder(importDetails.DataDirectory);
            return OpenConnection(new Sage50ImportDetails
            {
                DataDirectory = folder,
                Password = importDetails.Password,
                Username = importDetails.Username
            }, drivers);
        }

        private string GetFolder(string dataDirectory)
        {
            if (!fileSystem.DirectoryExists(dataDirectory))
            {
                throw new IncorrectSage50CredentialsException(String.Format("The directory {0} does not exist. Enter a directory which is a Sage 50 data directory and try again", dataDirectory));
            }

            var accdata = Path.Combine(dataDirectory, "ACCDATA");
            return fileSystem.DirectoryExists(accdata) ? accdata : dataDirectory;
        }

        private DbConnection OpenConnection(Sage50ImportDetails importDetails, IEnumerable<Sage50Driver> drivers)
        {
            foreach (var driver in drivers)
            {
                try
                {
                    return OpenConnection(importDetails, driver);
                }
                catch (CannotOpenDataDirectoryException)
                {
                }
            }

            throw new IncorrectSage50CredentialsException(
                "Could not open the specified folder with available versions of Sage 50.\n" +
                "Check that the folder is a Sage 50 data directory and that the correct version of Sage is installed.\n" +
                "The data directory can be found by logging in to Sage and clicking help->about from the menu.\n" +
                "Available versions of Sage 50: " + String.Join(", ", drivers.Select(x => x.FriendlyName)));
        }

        private DbConnection OpenConnection(Sage50ImportDetails importDetails, Sage50Driver sage50Driver)
        {
            var connectionString = CreateConnectionString(importDetails, sage50Driver);
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
                    throw new IncorrectSage50CredentialsException("Incorrect username or password");
                }

                throw;
            }
        }

        private static string CreateConnectionString(Sage50ImportDetails importDetails, Sage50Driver driver)
        {
            var builder = new OdbcConnectionStringBuilder
            {
                Driver = driver.Name
            };

            builder["uid"] = importDetails.Username;
            builder["dir"] = importDetails.DataDirectory;

            if (!string.IsNullOrEmpty(importDetails.Password))
            {
                builder["pwd"] = importDetails.Password;
            }

            var connectionString = builder.ConnectionString;
            return connectionString;
        }
    }

    internal class CannotOpenDataDirectoryException : Exception
    {
    }
}