using Model.SearchWindows;
using Persistence;
using Searching;

namespace Xero
{
    public class XeroSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(IJournalRepository repository)
        {            
            return new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new NotSupportedSearcher<UserParameters>("Xero does not record who raises individual journals"));
        }

        public SearchCapability GetSearchCapability()
        {
            return new SearchCapability(new []{SearchField.description, SearchField.username}, new []{SearchAction.users});
        }
    }
}