using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using Model;
using Model.Accounting;
using Native;
using Native.Disk;
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

        public void WriteTransactions(string description, IEnumerable<Transaction> transactions, string filename, IEnumerable<DisplayField> availableFields)
        {
            var fields = new HashSet<DisplayField>(availableFields);
            WriteTransactions(description, converter.ConvertToTabularFormat(transactions), filename, GetColumns(fields));
        }

        private List<ICsvColumn<SqlLedgerEntry>> GetColumns(ICollection<DisplayField> fields)
        {
            return columnFactories.Select(x => x.GetColumn(fields)).ToList();
        }

        private void WriteTransactions(string description, IEnumerable<SqlLedgerEntry> transactions, string filename, IList<ICsvColumn<SqlLedgerEntry>> columns)
        {
            using (var writer = writerFactory.CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, columns);
                foreach (var transaction in transactions)
                {
                    WriteTransaction(writer, transaction, columns);
                }
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
