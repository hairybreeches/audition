using Model.SearchWindows;
using Persistence;
using Searching;

namespace Xero
{
    public class XeroSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(JournalRepository repository)
        {            
            return new JournalSearcher(
                new WorkingHoursSearcher(repository),
                new YearEndSearcher(repository),
                new UnusualAccountsSearcher(repository),
                new RoundNumberSearcher(repository),
                new NotSupportedSearcher<UserParameters>("Xero does not record who raises individual journals"));
        }       

    }
}