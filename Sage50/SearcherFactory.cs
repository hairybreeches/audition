using System;
using System.Data.Odbc;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Searching;

namespace Sage50
{
    public class SearcherFactory
    {
        private readonly Func<SageConnectionFactory, Sage50JournalSearcher> journalSearcherFactory;

        public SearcherFactory(Func<SageConnectionFactory, Sage50JournalSearcher> journalSearcherFactory)
        {
            this.journalSearcherFactory = journalSearcherFactory;
        }

        public IJournalSearcher CreateJournalSearcher(Sage50LoginDetails loginDetails)
        {
            return journalSearcherFactory(CreateConnectionFactory(loginDetails));
        }

        private static SageConnectionFactory CreateConnectionFactory(Sage50LoginDetails loginDetails)
        {
            var connectionString = CreateConnectionString(loginDetails);
            var factory = new SageConnectionFactory(connectionString);
            TestFactory(factory);
            return factory;
        }

        private static void TestFactory(SageConnectionFactory loginDetails)
        {
            using (var connection = loginDetails.OpenConnection())
            {
                connection.GetSchema();
            }
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

