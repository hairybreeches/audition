using System.Data.Odbc;
using Model;
using Model.Persistence;
using Model.Searching;

namespace Sage50
{
    public class Sage50SearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(InMemoryJournalRepository repository)
        {
            return new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new UserSearcher(repository));
        }
    }
}

