using System.Collections.Generic;
using System.Linq;
using Capabilities;
using Model;
using Model.Accounting;
using SqlImport;

namespace CsvExport
{
    public class CsvExporter : ITransactionExporter
    {
        private readonly TabularFormatConverter converter;
        private readonly ISpreadsheetWriterFactory writerFactory;


        public CsvExporter(TabularFormatConverter converter, ISpreadsheetWriterFactory writerFactory)
        {
            this.converter = converter;
            this.writerFactory = writerFactory;
        }

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, IList<DisplayField> availableFields)
        {            
            Export(description, converter.ConvertToTabularFormat(transactions), filename, availableFields);
        }       

        private void Export(string description, IEnumerable<SqlLedgerEntry> transactions, string filename, IList<DisplayField> columns)
        {
            using (var writer = writerFactory.CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, columns);
                WriteTransactions(transactions, columns, writer);
            }
        }

        private static void WriteTransactions(IEnumerable<SqlLedgerEntry> transactions, IList<DisplayField> columns, ISpreadsheetWriter writer)
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

        private static void WriteHeaderRow(ISpreadsheetWriter writer, IList<DisplayField> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                writer.WriteField(column.GetHeaderValue());
            }
            writer.NextRecord();
        }

        private static void WriteTransaction(ISpreadsheetWriter writer, SqlLedgerEntry transaction, IEnumerable<DisplayField> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                writer.WriteField(column.GetDisplayValue(transaction));
            }
            writer.NextRecord();
        }             
    }
}
