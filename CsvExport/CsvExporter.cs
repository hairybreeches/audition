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
        private readonly IEnumerable<IColumnFactory> columnFactories;

        private readonly TabularFormatConverter converter;
        private readonly ISpreadsheetWriterFactory writerFactory;


        public CsvExporter(TabularFormatConverter converter, ISpreadsheetWriterFactory writerFactory, IEnumerable<IColumnFactory> columnFactories)
        {
            this.converter = converter;
            this.writerFactory = writerFactory;
            this.columnFactories = columnFactories;
        }

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, ICollection<DisplayField> availableFields)
        {            
            Export(description, converter.ConvertToTabularFormat(transactions), filename, GetColumns(availableFields));
        }

        private List<ICsvColumn> GetColumns(ICollection<DisplayField> fields)
        {
            return columnFactories.Select(x => x.GetColumn(fields)).ToList();
        }

        private void Export(string description, IEnumerable<SqlLedgerEntry> transactions, string filename, IList<ICsvColumn> columns)
        {
            using (var writer = writerFactory.CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, columns);
                WriteTransactions(transactions, columns, writer);
            }
        }

        private static void WriteTransactions(IEnumerable<SqlLedgerEntry> transactions, IList<ICsvColumn> columns, ISpreadsheetWriter writer)
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

        private static void WriteHeaderRow(ISpreadsheetWriter writer, IList<ICsvColumn> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                column.WriteHeader(writer);
            }
            writer.NextRecord();
        }

        private static void WriteTransaction(ISpreadsheetWriter writer, SqlLedgerEntry transaction, IEnumerable<ICsvColumn> transactionColumns)
        {
            foreach (var column in transactionColumns)
            {
                column.WriteField(writer, transaction);
            }
            writer.NextRecord();
        }             
    }
}
