using System.Data.Odbc;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Sage50
{
    public class SearcherFactory
    {
        public IJournalSearcher CreateJournalSearcher(Sage50LoginDetails loginDetails)
        {
            TestLoginDetails(loginDetails);
            return new JournalSearcher(loginDetails);
        }

        private static void TestLoginDetails(Sage50LoginDetails loginDetails)
        {
            using (var connection = loginDetails.OpenConnection())
            {
                connection.GetSchema();
            }
        }
    }
}

