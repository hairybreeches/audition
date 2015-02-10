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
        public IQueryable<Journal> FindJournalsWithin(UnusualAccountsParameters parameters, IQueryable<Journal> journals)
        {
            var lookup = new AccountsLookup(journals);
            var unusualAccountCodes = lookup.UnusualAccountCodes(parameters.MinimumEntriesToBeConsideredNormal);
            return journals
                .Where(journal=>journal.Lines.Any(journalLine => unusualAccountCodes.Contains(journalLine.AccountCode)));
        }
    }
}