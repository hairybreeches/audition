using System;
using SqlImport;

namespace CsvExport
{
    internal class CsvColumn : ICsvColumn
    {
        private readonly string header;
        private readonly Func<SqlLedgerEntry, object> fieldSelector;

        public CsvColumn(string header, Func<SqlLedgerEntry, object> fieldSelector)
        {
            this.header = header;
            this.fieldSelector = fieldSelector;
        }

        public void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record)
        {
            writer.WriteField(fieldSelector(record));
        }

        public void WriteHeader(ISpreadsheetWriter writer)
        {
            writer.WriteField(header);
        }
    }
}