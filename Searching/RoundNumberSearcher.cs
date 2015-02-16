using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class RoundNumberSearcher : ISearcher<EndingParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(EndingParameters parameters, IQueryable<Transaction> transactions)
        {           
            var magnitude = parameters.Magnitude();
            return transactions.Where(journal => HasRoundLine(journal, magnitude));
        }

        private bool HasRoundLine(Transaction transaction, int magnitude)
        {
            return transaction.Lines.Any(line => ContainsRoundValue(line, magnitude));
        }

        private bool ContainsRoundValue(LedgerEntry line, int magnitude)
        {
            return IsRound(line.Amount, magnitude)
                   || IsRound(line.Amount, magnitude)
                   || IsRound(line.Amount, magnitude);
        }

        public bool IsRound(decimal amount, int magnitude)
        {
            var pence = amount * 100;
            return pence != 0 && pence % magnitude == 0;
        }               
    }
}