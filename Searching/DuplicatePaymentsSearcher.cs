using System.Linq;
using Model.Accounting;
using Searching.SearchWindows;

namespace Searching
{
    class DuplicatePaymentsSearcher : ISearcher<DuplicatePaymentsParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(DuplicatePaymentsParameters parameters, IQueryable<Transaction> transactions)
        {
            return transactions;
        }
    }
}