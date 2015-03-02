using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using Searching.SearchWindows;
using SqlImport;

namespace Searching
{
    class DuplicatePaymentsSearcher : ISearcher<DuplicatePaymentsParameters>
    {

        public IQueryable<Transaction> FindTransactionsWithin(DuplicatePaymentsParameters parameters, IQueryable<Transaction> transactions)
        {
            var converter = new TabularFormatConverter();
            var sqlLedgerEntries = transactions
                .SelectMany(converter.ConvertToTabularFormat)
                .Where(x=>x.Amount != 0)
                .GroupBy(x=> new PaymentProperties(x))
                .SelectMany(group => FindTransactionWithinDays(group, parameters.MaximumDaysBetweenTransactions));

            return converter.ReadTransactions(
                sqlLedgerEntries)
                .AsQueryable();
        }

        private IEnumerable<SqlLedgerEntry> FindTransactionWithinDays(IEnumerable<SqlLedgerEntry> entries, int maximumDaysBetweenTransactions)
        {
            var sortedEntries = entries.OrderBy(x => x.TransactionDate);
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

        private bool AreWithinDays(SqlLedgerEntry entry1, SqlLedgerEntry entry2, int numberOfDays)
        {
            var difference = entry2.TransactionDate - entry1.TransactionDate;
            return difference.TotalDays <= numberOfDays;
        }

        private class PaymentProperties
        {
            public PaymentProperties(SqlLedgerEntry entry)
            {
                NominalCode = entry.NominalCode;
                Amount = entry.Amount;
            }

            public string NominalCode { get; private set; }
            public decimal Amount { get; private set; }

            protected bool Equals(PaymentProperties other)
            {
                return string.Equals(NominalCode, other.NominalCode) && Amount == other.Amount;
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
                    return ((NominalCode != null ? NominalCode.GetHashCode() : 0)*397) ^ Amount.GetHashCode();
                }
            }
        }
    }
}