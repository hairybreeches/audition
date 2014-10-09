using System.ComponentModel.DataAnnotations;
using System.Data.Odbc;

namespace Sage50
{
    public class Sage50LoginDetails
    {        
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DataDirectory { get; set; }


        public OdbcConnection OpenConnection()
        {
            var builder = new OdbcConnectionStringBuilder
            {
                Dsn = "SageDemo",
            };

            builder["uid"] = Username;

            if (!string.IsNullOrEmpty(Password))
            {
                builder["pwd"] = Password;
            }

            var conn = new OdbcConnection(builder.ConnectionString);
            conn.Open();
            return conn;
        }
    }
}