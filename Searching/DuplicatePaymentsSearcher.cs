using System;
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
        private readonly TabularFormatConverter tabularFormatConverter;

        public DuplicatePaymentsSearcher(TabularFormatConverter formatConverter)
        {
            tabularFormatConverter = formatConverter;
        }

        public IQueryable<Transaction> FindTransactionsWithin(DuplicatePaymentsParameters parameters, IQueryable<Transaction> transactions)
        {
            return transactions
                .SelectMany(Project)
                .Where(x=>x.ThisEntry.Amount != 0)
                .GroupBy(x=> new PaymentProperties(x.ThisEntry))
                .SelectMany(group => FindTransactionWithinDays(group, parameters.MaximumDaysBetweenTransactions))
                .Select(x=>tabularFormatConverter.CreateTransaction(x.AllEntries))
                .AsQueryable();
        }

        private IEnumerable<TransactionProjection> Project(Transaction transaction)
        {
            var lines = tabularFormatConverter.ConvertToTabularFormat(transaction).ToList();
            return lines.Select(x=>new TransactionProjection(x, lines));
        }

        private IEnumerable<TransactionProjection> FindTransactionWithinDays(IEnumerable<TransactionProjection> entries, int maximumDaysBetweenTransactions)
        {
            var sortedEntries = entries.OrderBy(x => x.ThisEntry.TransactionDate);
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
            var difference = entry2.ThisEntry.TransactionDate - entry1.ThisEntry.TransactionDate;
            return difference.TotalDays <= numberOfDays;
        }

        private class TransactionProjection
        {
            public TransactionProjection(SqlLedgerEntry thisEntry, IEnumerable<SqlLedgerEntry> allEntries)
            {
                ThisEntry = thisEntry;
                AllEntries = allEntries;
            }

            public SqlLedgerEntry ThisEntry { get; private set; }
            public IEnumerable<SqlLedgerEntry> AllEntries { get; private set; }
        }

        private class PaymentProperties
        {
            public PaymentProperties(SqlLedgerEntry entry)
            {
                NominalCode = entry.NominalCode;
                Amount = entry.Amount;
            }

            private string NominalCode { get; set; }
            private decimal Amount { get; set; }

            private bool Equals(PaymentProperties other)
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