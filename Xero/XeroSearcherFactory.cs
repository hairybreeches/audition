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
            return new SearchCapability(
            new[]{
            SearchField.AccountCode, 
            SearchField.AccountName, 
            SearchField.Amount,
            SearchField.Created, 
            SearchField.JournalDate, 
            SearchField.JournalType
            },
            new[]
            {
                SearchAction.Accounts, 
                SearchAction.Date, 
                SearchAction.Ending, 
                SearchAction.Hours
            });
        }
    }
}