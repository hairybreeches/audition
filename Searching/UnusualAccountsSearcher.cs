using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class UnusualAccountsSearcher : ISearcher<UnusualAccountsParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(UnusualAccountsParameters parameters, IQueryable<Transaction> transactions)
        {
            var lookup = new AccountsLookup(transactions);
            var unusualAccountCodes = lookup.UnusualNominalCodes(parameters.MinimumEntriesToBeConsideredNormal);
            return transactions
                .Where(transaction=>transaction.Lines.Any(line => unusualAccountCodes.Contains(line.NominalCode)));
        }
    }
}