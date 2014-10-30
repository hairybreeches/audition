using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class UnusualAccountsSearcher : IJournalSearcher<UnusualAccountsParameters>
    {
        private readonly JournalRepository repository;

        public UnusualAccountsSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }   

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period).ToList();
            var lookup = new AccountsLookup(periodJournals);
            return lookup.JournalsMadeToUnusualAccountCodes(searchWindow.Parameters.MinimumEntriesToBeConsideredNormal);
        }
    }
}