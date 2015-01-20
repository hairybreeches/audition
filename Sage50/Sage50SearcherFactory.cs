using Persistence;
using Searching;

namespace Sage50
{
    //todo: duplicate information between the two methods (same for all other implementations of this interface)
    public class Sage50SearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(IJournalRepository repository)
        {
            return new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new UserSearcher(repository));
        }

        public SearchCapability GetSearchCapability()
        {
            return SearchCapability.EverythingAvailable;
        }
    }
}

