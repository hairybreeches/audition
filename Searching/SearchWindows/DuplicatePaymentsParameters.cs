using System.Linq;
using Model.Accounting;

namespace Searching.SearchWindows
{
    public class DuplicatePaymentsParameters : ISearchParameters
    {
        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }
    }
}