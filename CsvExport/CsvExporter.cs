using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using Model;
using Model.Accounting;
using Native;
using Native.Disk;

namespace CsvExport
{
    public class CsvExporter : ITransactionExporter
    {
        private readonly IFileSystem fileSystem;

        private readonly IEnumerable<ColumnFactory<Transaction>> transactionColumnFactories = new[]
        {
            new ColumnFactory<Transaction>("Created", DisplayField.Created, transaction => transaction.Created),
            new ColumnFactory<Transaction>("Date", DisplayField.TransactionDate, transaction => transaction.TransactionDate.ToShortDateString()),
            new ColumnFactory<Transaction>("Username", DisplayField.Username, transaction => transaction.Username),
            new ColumnFactory<Transaction>("Description", DisplayField.Description, transaction => transaction.Description)
        };
        
        private readonly IEnumerable<ColumnFactory<LedgerEntry>> ledgerEntryColumnFactories = new[]
        {
            new ColumnFactory<LedgerEntry>("", DisplayField.LedgerEntryType, line => line.LedgerEntryType),
            new ColumnFactory<LedgerEntry>("", DisplayField.AccountCode, line => line.AccountCode),
            new ColumnFactory<LedgerEntry>("", DisplayField.AccountName, line => line.AccountName),
            new ColumnFactory<LedgerEntry>("", DisplayField.Amount, line => line.Amount)
        };



        public CsvExporter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteTransactions(string description, IEnumerable<Transaction> transactions, string filename, IEnumerable<DisplayField> availableFields)
        {
            var fields = new HashSet<DisplayField>(availableFields);
            WriteTransactions(description, transactions, filename, GetColumns(fields, transactionColumnFactories), GetColumns(fields, ledgerEntryColumnFactories));
        }

        private IList<ICsvColumn<T>> GetColumns<T>(ICollection<DisplayField> fields, IEnumerable<ColumnFactory<T>> columnFactories)
        {
            return columnFactories.Select(x => x.GetColumn(fields)).ToList();
        }

        private void WriteTransactions(string description, IEnumerable<Transaction> transactions, string filename, IList<ICsvColumn<Transaction>> transactionColumns, IList<ICsvColumn<LedgerEntry>> ledgerEntryColumns)
        {
            using (var writer = CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, transactionColumns);
                foreach (var transaction in transactions)
                {
                    WriteTransaction(writer, transaction, transactionColumns, ledgerEntryColumns);
                }
            }
        }

        private static void WriteDescriptionRow(ICsvWriter writer, string description)
        {
            writer.WriteField(description);
            writer.NextRecord();
        }

        private static void WriteHeaderRow(ICsvWriter writer, IEnumerable<ICsvColumn<Transaction>> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                column.WriteHeader(writer);
            }
            writer.NextRecord();
        }

        private static void WriteTransaction(ICsvWriter writer, Transaction transaction, IEnumerable<ICsvColumn<Transaction>> transactionColumns, IEnumerable<ICsvColumn<LedgerEntry>> ledgerEntryColumns)
        {
            foreach (var column in transactionColumns)
            {
                column.WriteField(writer, transaction);
            }

            foreach (var line in transaction.Lines)
            {
                WriteLedgerEntry(writer, ledgerEntryColumns, line);
            }
            writer.NextRecord();
        }

        private static void WriteLedgerEntry(ICsvWriter writer, IEnumerable<ICsvColumn<LedgerEntry>> ledgerEntryColumns, LedgerEntry line)
        {
            foreach (var column in ledgerEntryColumns)
            {
                column.WriteField(writer, line);
            }
        }

        private ICsvWriter CreateWriter(string filename)
        {
            return new CsvWriter(fileSystem.OpenFileToWrite(filename));
        }        
    }

    public class ColumnFactory<TRecord>
    {
        private readonly DisplayField displayField;
        private readonly string header;
        private readonly Func<TRecord, object> fieldSelector;

        public ColumnFactory(string header, DisplayField displayField, Func<TRecord, object> fieldSelector)
        {
            this.header = header;
            this.displayField = displayField;
            this.fieldSelector = fieldSelector;
        }

        internal ICsvColumn<TRecord> GetColumn(ICollection<DisplayField> availableFields)
        {
            if (availableFields.Contains(displayField))
            {
                return new CsvColumn<TRecord>(header, fieldSelector);
            }
            else
            {
                return new NullCsvColumn<TRecord>();
            }
        }


    }
}
