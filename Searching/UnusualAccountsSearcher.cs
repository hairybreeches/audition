using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class UnusualAccountsSearcher : IJournalSearcher<UnusualAccountsParameters>
    {
        private readonly IJournalRepository repository;

        public UnusualAccountsSearcher(IJournalRepository repository)
        {
            this.repository = repository;
        }   

        public IQueryable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            var lookup = new AccountsLookup(repository.GetJournalsApplyingTo(searchWindow.Period));
            var unusualAccountCodes = lookup.UnusualAccountCodes(searchWindow.Parameters.MinimumEntriesToBeConsideredNormal);
            return repository.GetJournalsApplyingTo(searchWindow.Period)
                .Where(journal=>journal.Lines.Any(journalLine => unusualAccountCodes.Contains(journalLine.AccountCode)));
        }
    }
}