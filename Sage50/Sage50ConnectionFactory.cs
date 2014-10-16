using System.Data.Odbc;

namespace Sage50
{
    public class Sage50ConnectionFactory
    {
        public OdbcConnection OpenConnection(Sage50LoginDetails loginDetails)
        {
            var connectionString = CreateConnectionString(loginDetails);
            var conn = new OdbcConnection(connectionString);
            conn.Open();
            return conn;
        }

        private static string CreateConnectionString(Sage50LoginDetails loginDetails)
        {
            var builder = new OdbcConnectionStringBuilder
            {
                Driver = "Sage Line 50 v21"
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