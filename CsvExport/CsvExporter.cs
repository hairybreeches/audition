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

        private readonly IEnumerable<ColumnFactory<Journal>> journalColumnFactories = new[]
        {
            new ColumnFactory<Journal>("Created", DisplayField.Created, journal => journal.Created),
            new ColumnFactory<Journal>("Date", DisplayField.JournalDate, journal => journal.JournalDate.ToShortDateString()),
            new ColumnFactory<Journal>("Username", DisplayField.Username, journal => journal.Username),
            new ColumnFactory<Journal>("Description", DisplayField.Description, journal => journal.Description)
        };
        
        private readonly IEnumerable<ColumnFactory<JournalLine>> journalLineColumnFactories = new[]
        {
            new ColumnFactory<JournalLine>("", DisplayField.JournalType, line => line.JournalType),
            new ColumnFactory<JournalLine>("", DisplayField.AccountCode, line => line.AccountCode),
            new ColumnFactory<JournalLine>("", DisplayField.AccountName, line => line.AccountName),
            new ColumnFactory<JournalLine>("", DisplayField.Amount, line => line.Amount)
        };



        public CsvExporter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteJournals(string description, IEnumerable<Journal> journals, string filename, IEnumerable<DisplayField> availableFields)
        {
            var fields = new HashSet<DisplayField>(availableFields);
            WriteJournals(description, journals, filename, GetColumns(fields, journalColumnFactories), GetColumns(fields, journalLineColumnFactories));
        }

        private IList<ICsvColumn<T>> GetColumns<T>(ICollection<DisplayField> fields, IEnumerable<ColumnFactory<T>> columnFactories)
        {
            return columnFactories.Select(x => x.GetColumn(fields)).ToList();
        }

        private void WriteJournals(string description, IEnumerable<Journal> journals, string filename, IList<ICsvColumn<Journal>> journalColumns, IList<ICsvColumn<JournalLine>> journalLineColumns)
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

        private static void WriteHeaderRow(ICsvWriter writer, IEnumerable<ICsvColumn<Journal>> journalColumns)
        {
            foreach (var journalColumn in journalColumns)
            {
                journalColumn.WriteHeader(writer);
            }
            writer.NextRecord();
        }

        private static void WriteJournal(ICsvWriter writer, Journal journal, IEnumerable<ICsvColumn<Journal>> journalColumns, IEnumerable<ICsvColumn<JournalLine>> journalLineColumns)
        {
            foreach (var journalColumn in journalColumns)
            {
                journalColumn.WriteField(writer, journal);
            }

            foreach (var line in journal.Lines)
            {
                WriteLine(writer, journalLineColumns, line);
            }
            writer.NextRecord();
        }

        private static void WriteLine(ICsvWriter writer, IEnumerable<ICsvColumn<JournalLine>> journalLineColumns, JournalLine line)
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
