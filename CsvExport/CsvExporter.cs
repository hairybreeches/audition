﻿using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using SqlImport;

namespace CsvExport
{
    public class CsvExporter : ITransactionExporter
    {
        private readonly IEnumerable<ColumnFactory<SqlLedgerEntry>> columnFactories;

        private readonly TabularFormatConverter converter;
        private readonly ISpreadsheetWriterFactory writerFactory;


        public CsvExporter(TabularFormatConverter converter, ISpreadsheetWriterFactory writerFactory, IEnumerable<ColumnFactory<SqlLedgerEntry>> columnFactories)
        {
            this.converter = converter;
            this.writerFactory = writerFactory;
            this.columnFactories = columnFactories;
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
