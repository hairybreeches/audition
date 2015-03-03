using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class UnusualAccountsSearcher : ISearcher<UnusualNominalCodesParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(UnusualNominalCodesParameters parameters, IQueryable<Transaction> transactions)
        {
            var lookup = new NominalCodeLookup(transactions);
            var unusualNominalCodes = lookup.UnusualNominalCodes(parameters.MinimumEntriesToBeConsideredNormal);
            return transactions
                .Where(transaction=>transaction.Lines.Any(line => unusualNominalCodes.Contains(line.NominalCode)));
        }
    }
}