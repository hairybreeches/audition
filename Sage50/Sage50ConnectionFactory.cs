using System.Data.Odbc;

namespace Sage50
{
    public class Sage50ConnectionFactory : ISage50ConnectionFactory
    {
        private readonly Sage50DriverDetector driverDetector;

        public Sage50ConnectionFactory(Sage50DriverDetector driverDetector)
        {
            this.driverDetector = driverDetector;
        }

        public OdbcConnection OpenConnection(Sage50LoginDetails loginDetails)
        {
            var connectionString = CreateConnectionString(loginDetails, driverDetector.FindBestDriver());
            var conn = new OdbcConnection(connectionString);
            conn.Open();
            return conn;
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
}