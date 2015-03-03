using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Searching.SearchWindows;

namespace Searching
{
    class DuplicatePaymentsSearcher : ISearcher<DuplicatePaymentsParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(DuplicatePaymentsParameters parameters, IQueryable<Transaction> transactions)
        {
            return transactions
                .SelectMany(Project)
                .Where(x=>x.LineProperties.Amount != 0)
                .GroupBy(x=> x.LineProperties)
                .SelectMany(group => FindTransactionWithinDays(group, parameters.MaximumDaysBetweenTransactions))
                .Select(x=>x.Transaction)
                .AsQueryable();
        }

        private IEnumerable<TransactionProjection> Project(Transaction transaction)
        {
            return transaction.Lines.Select(x=>new TransactionProjection(new PaymentProperties(x), transaction));
        }

        private IEnumerable<TransactionProjection> FindTransactionWithinDays(IEnumerable<TransactionProjection> entries, int maximumDaysBetweenTransactions)
        {
            var sortedEntries = entries.OrderBy(x => x.Transaction.TransactionDate);
            var previous = sortedEntries.First();
            var previousReturned = false;
            foreach (var sqlLedgerEntry in sortedEntries.Skip(1))
            {
                if (AreWithinDays(previous, sqlLedgerEntry, maximumDaysBetweenTransactions))
                {
                    if (!previousReturned)
                    {
                        yield return previous;
                    }
                    yield return sqlLedgerEntry;

                    previousReturned = true;                    
                }
                else
                {
                    previousReturned = false;
                }

                previous = sqlLedgerEntry;
            }
        }

        private bool AreWithinDays(TransactionProjection entry1, TransactionProjection entry2, int numberOfDays)
        {
            var difference = entry2.Transaction.TransactionDate - entry1.Transaction.TransactionDate;
            return difference.TotalDays <= numberOfDays;
        }

        private class TransactionProjection
        {
            public TransactionProjection(PaymentProperties paymentProperties, Transaction transaction)
            {
                LineProperties = paymentProperties;
                Transaction = transaction;
            }

            public PaymentProperties LineProperties { get; private set; }
            public Transaction Transaction { get; private set; }
        }

        private class PaymentProperties
        {
            public PaymentProperties(LedgerEntry entry)
            {
                NominalCode = entry.NominalCode;
                Amount = entry.Amount;
                LedgerEntryType = entry.LedgerEntryType;
            }

            private string NominalCode { get; set; }
            public decimal Amount { get; private set; }
            private LedgerEntryType LedgerEntryType { get; set; }

            private bool Equals(PaymentProperties other)
            {
                return string.Equals(NominalCode, other.NominalCode) && Amount == other.Amount && LedgerEntryType == other.LedgerEntryType;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PaymentProperties) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (NominalCode != null ? NominalCode.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ Amount.GetHashCode();
                    hashCode = (hashCode*397) ^ (int) LedgerEntryType;
                    return hashCode;
                }
            }
        }
    }
}