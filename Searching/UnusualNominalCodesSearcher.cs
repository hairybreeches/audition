using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Searching.SearchWindows;

namespace Searching
{
    public class UnusualNominalCodesSearcher : ISearcher<UnusualNominalCodesParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(UnusualNominalCodesParameters parameters, IQueryable<Transaction> transactions)
        {
            var lookup = new NominalCodeLookup(transactions);
            var unusualNominalCodes = lookup.UnusualNominalCodes(parameters.MinimumEntriesToBeConsideredNormal);
            return transactions
                .SelectMany(transaction => GetTransactionAndReason(transaction, unusualNominalCodes))
                .OrderBy(x=>x.Reason)
                .Select(x=>x.Transaction);
        }

        private IEnumerable<TransactionAndReason> GetTransactionAndReason(Transaction transaction, ISet<string> unusualNominalCodes)
        {            
            foreach (var line in transaction.Lines)
            {
                var nominalCode = line.NominalCode;
                if (unusualNominalCodes.Contains(nominalCode))
                {
                    yield return new TransactionAndReason(transaction, nominalCode);
                }
            }
        }

        private class TransactionAndReason
        {
            public TransactionAndReason(Transaction transaction, string reason)
            {
                Transaction = transaction;
                Reason = reason;
            }

            public Transaction Transaction {get; private set; }
            public string Reason {get; private set; }
        }
    }
}