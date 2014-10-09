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
                Driver = "Sage Line 50 v21"
            };

            builder["uid"] = Username;
            builder["dir"] = DataDirectory;

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