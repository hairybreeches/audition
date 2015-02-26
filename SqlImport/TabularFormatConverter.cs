using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;

namespace SqlImport
{
    /// <summary>
    /// Knows how to turn tabular data format SqlLedgerEntries into Transactions and back again    
    /// </summary>
    public class TabularFormatConverter
    {
        internal IEnumerable<Transaction> ReadTransactions(IEnumerable<SqlLedgerEntry> lines)
        {
            var grouped = lines.GroupBy(x => x.TransactionId);
            return grouped.Select(CreateTransaction);
        }

        public IEnumerable<SqlLedgerEntry> ConvertToTabularFormat(IEnumerable<Transaction> transactions)
        {
            return transactions.SelectMany(ConvertToTabularFormat);
        }

        public IEnumerable<SqlLedgerEntry> ConvertToTabularFormat(Transaction transaction)
        {
            return transaction.Lines.Select(ledgerEntry => ConvertToTabularFormat(transaction, ledgerEntry));
        }

        private static SqlLedgerEntry ConvertToTabularFormat(Transaction transaction, LedgerEntry ledgerEntry)
        {
            return new SqlLedgerEntry(transaction.Id, transaction.Username, transaction.TransactionDate, ledgerEntry.AccountCode, ledgerEntry.Amount, ledgerEntry.LedgerEntryType, transaction.Description, ledgerEntry.AccountName, transaction.TransactionType);
        }

        private Transaction CreateTransaction(IGrouping<string, SqlLedgerEntry> lines)
        {
            var ledgerEntries = lines.ToList();
            return new Transaction(
                lines.Key,
                GetField(ledgerEntries, x => x.TransactionDate, MappingField.TransactionDate),
                GetField(ledgerEntries, x => x.Username, MappingField.Username),
                GetField(ledgerEntries, x => x.Description, MappingField.Description),
                ledgerEntries.Select(ToModelLine),
                GetField(ledgerEntries, x => x.TransactionType, MappingField.Type));


        }

        private static LedgerEntry ToModelLine(SqlLedgerEntry arg)
        {
            return new LedgerEntry(arg.NominalCode, arg.NominalCodeName, arg.LedgerEntryType, arg.Amount);
        }

        private static T GetField<T>(IList<SqlLedgerEntry> ledgerEntries, Func<SqlLedgerEntry, T> getter, MappingField mappingField)
        {
            var values = ledgerEntries.Select(getter).Distinct().ToList();
            if (values.Count > 1)
            {
                throw new SqlDataFormatUnexpectedException(String.Format("Expected only one value for {0} per transaction. Actual values for transaction with id {1}: {2}. This can happen if you assign the 'ID' column incorrectly when importing data from Excel.",
                    mappingField, ledgerEntries.First().TransactionId, ValuesString(values)));
            }
            return values.Single();
        }

        private static string ValuesString<T>(IList<T> values)
        {
            const int maxListLength = 5;

            var valuesString = String.Join(", ", values.Take(maxListLength));
            if (values.Count > maxListLength)
            {
                valuesString += String.Format("and {0} more", values.Count - maxListLength);
            }
            return valuesString;
        }
    }
}