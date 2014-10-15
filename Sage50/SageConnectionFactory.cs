using System.Data.Odbc;

namespace Sage50
{
    public class SageConnectionFactory
    {
        private readonly string connectionString;

        public SageConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public OdbcConnection OpenConnection()
        {
            var conn = new OdbcConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}