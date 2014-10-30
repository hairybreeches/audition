using Model.Persistence;
using Searching;

namespace Sage50
{
    public class Sage50SearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(JournalRepository repository)
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

