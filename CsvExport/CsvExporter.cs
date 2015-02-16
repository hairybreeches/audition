using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using Model;
using Model.Accounting;
using Native;

namespace CsvExport
{
    public class CsvExporter : IJournalExporter
    {
        private readonly IFileSystem fileSystem;

        private readonly IEnumerable<ColumnFactory<Transaction>> journalColumnFactories = new[]
        {
            new ColumnFactory<Transaction>("Created", DisplayField.Created, journal => journal.Created),
            new ColumnFactory<Transaction>("Date", DisplayField.TransactionDate, journal => journal.TransactionDate.ToShortDateString()),
            new ColumnFactory<Transaction>("Username", DisplayField.Username, journal => journal.Username),
            new ColumnFactory<Transaction>("Description", DisplayField.Description, journal => journal.Description)
        };
        
        private readonly IEnumerable<ColumnFactory<LedgerEntry>> journalLineColumnFactories = new[]
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

        public void WriteJournals(string description, IEnumerable<Transaction> journals, string filename, IEnumerable<DisplayField> availableFields)
        {
            var fields = new HashSet<DisplayField>(availableFields);
            WriteJournals(description, journals, filename, GetColumns(fields, journalColumnFactories), GetColumns(fields, journalLineColumnFactories));
        }

        private IList<ICsvColumn<T>> GetColumns<T>(ICollection<DisplayField> fields, IEnumerable<ColumnFactory<T>> columnFactories)
        {
            return columnFactories.Select(x => x.GetColumn(fields)).ToList();
        }

        private void WriteJournals(string description, IEnumerable<Transaction> journals, string filename, IList<ICsvColumn<Transaction>> journalColumns, IList<ICsvColumn<LedgerEntry>> journalLineColumns)
        {
            using (var writer = CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, journalColumns);
                foreach (var journal in journals)
                {
                    WriteJournal(writer, journal, journalColumns, journalLineColumns);
                }
            }
        }

        private static void WriteDescriptionRow(ICsvWriter writer, string description)
        {
            writer.WriteField(description);
            writer.NextRecord();
        }

        private static void WriteHeaderRow(ICsvWriter writer, IEnumerable<ICsvColumn<Transaction>> journalColumns)
        {
            foreach (var journalColumn in journalColumns)
            {
                journalColumn.WriteHeader(writer);
            }
            writer.NextRecord();
        }

        private static void WriteJournal(ICsvWriter writer, Transaction transaction, IEnumerable<ICsvColumn<Transaction>> journalColumns, IEnumerable<ICsvColumn<LedgerEntry>> journalLineColumns)
        {
            foreach (var journalColumn in journalColumns)
            {
                journalColumn.WriteField(writer, transaction);
            }

            foreach (var line in transaction.Lines)
            {
                WriteLine(writer, journalLineColumns, line);
            }
            writer.NextRecord();
        }

        private static void WriteLine(ICsvWriter writer, IEnumerable<ICsvColumn<LedgerEntry>> journalLineColumns, LedgerEntry line)
        {
            foreach (var journalLineColumn in journalLineColumns)
            {
                journalLineColumn.WriteField(writer, line);
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
