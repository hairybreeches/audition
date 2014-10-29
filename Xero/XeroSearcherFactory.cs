using System.Threading.Tasks;
using Model.Persistence;
using Model.Searching;
using Model.SearchWindows;

namespace Xero
{
    public class XeroSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(InMemoryJournalRepository repository)
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