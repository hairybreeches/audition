using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class UnusualAccountsSearcher : IJournalSearcher<UnusualAccountsParameters>
    {
        public IQueryable<Journal> FindJournalsWithin(UnusualAccountsParameters parameters, DateRange dateRange, IJournalRepository repository)
        {
            var lookup = new AccountsLookup(repository.GetJournalsApplyingTo(dateRange));
            var unusualAccountCodes = lookup.UnusualAccountCodes(parameters.MinimumEntriesToBeConsideredNormal);
            return repository.GetJournalsApplyingTo(dateRange)
                .Where(journal=>journal.Lines.Any(journalLine => unusualAccountCodes.Contains(journalLine.AccountCode)));
        }
    }
}