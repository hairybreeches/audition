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
        public IQueryable<Transaction> FindJournalsWithin(UnusualAccountsParameters parameters, IQueryable<Transaction> journals)
        {
            var lookup = new AccountsLookup(journals);
            var unusualAccountCodes = lookup.UnusualAccountCodes(parameters.MinimumEntriesToBeConsideredNormal);
            return journals
                .Where(journal=>journal.Lines.Any(journalLine => unusualAccountCodes.Contains(journalLine.AccountCode)));
        }
    }
}