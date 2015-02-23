using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace CsvExport
{
    public class CsvExporter : ITransactionExporter
    {
        private readonly IEnumerable<ColumnFactory<SqlLedgerEntry>> columnFactories = new[]
        {
            new ColumnFactory<SqlLedgerEntry>("Transaction ID", DisplayField.Id, line => line.TransactionId),
            new ColumnFactory<SqlLedgerEntry>("Entry time", DisplayField.Created, line => line.CreationTime),
            new ColumnFactory<SqlLedgerEntry>("Transaction date", DisplayField.TransactionDate, line => line.TransactionDate.ToShortDateString()),
            new ColumnFactory<SqlLedgerEntry>("Transaction type", DisplayField.Type, line => line.TransactionType),
            new ColumnFactory<SqlLedgerEntry>("Username", DisplayField.Username, line => line.Username),
            new ColumnFactory<SqlLedgerEntry>("Description", DisplayField.Description, line => line.Description),  
            new ColumnFactory<SqlLedgerEntry>("Dr/Cr", DisplayField.LedgerEntryType, line => line.LedgerEntryType),
            new ColumnFactory<SqlLedgerEntry>("Nominal Account", DisplayField.AccountCode, line => line.NominalCode),
            new ColumnFactory<SqlLedgerEntry>("Account name", DisplayField.AccountName, line => line.NominalCodeName),
            new ColumnFactory<SqlLedgerEntry>("Amount", DisplayField.Amount, line => line.Amount)
        };

        private readonly TabularFormatConverter converter;
        private readonly ISpreadsheetWriterFactory writerFactory;


        public CsvExporter(TabularFormatConverter converter, ISpreadsheetWriterFactory writerFactory)
        {
            this.converter = converter;
            this.writerFactory = writerFactory;
        }

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, IEnumerable<DisplayField> availableFields)
        {
            var fields = new HashSet<DisplayField>(availableFields);
            Export(description, converter.ConvertToTabularFormat(transactions), filename, GetColumns(fields));
        }

        private List<ICsvColumn<SqlLedgerEntry>> GetColumns(ICollection<DisplayField> fields)
        {
            return columnFactories.Select(x => x.GetColumn(fields)).ToList();
        }

        private void Export(string description, IEnumerable<SqlLedgerEntry> transactions, string filename, IList<ICsvColumn<SqlLedgerEntry>> columns)
        {
            using (var writer = writerFactory.CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, columns);
                WriteTransactions(transactions, columns, writer);
            }
        }

        private static void WriteTransactions(IEnumerable<SqlLedgerEntry> transactions, IList<ICsvColumn<SqlLedgerEntry>> columns, ISpreadsheetWriter writer)
        {
            foreach (var transaction in transactions)
            {
                WriteTransaction(writer, transaction, columns);
            }
        }

        private static void WriteDescriptionRow(ISpreadsheetWriter writer, string description)
        {
            writer.WriteField(description);
            writer.NextRecord();
        }

        private static void WriteHeaderRow(ISpreadsheetWriter writer, IList<ICsvColumn<SqlLedgerEntry>> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                column.WriteHeader(writer);
            }
            writer.NextRecord();
        }

        private static void WriteTransaction(ISpreadsheetWriter writer, SqlLedgerEntry transaction, IEnumerable<ICsvColumn<SqlLedgerEntry>> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                column.WriteField(writer, transaction);
            }
            writer.NextRecord();
        }             
    }
}
