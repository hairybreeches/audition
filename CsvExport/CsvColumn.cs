using System;
using Capabilities;
using Model;
using SqlImport;

namespace CsvExport
{
    public class CsvColumn : ICsvColumn
    {
        private readonly string header;
        private readonly DisplayField field;

        public CsvColumn(string header, DisplayField field)
        {
            this.header = header;
            this.field = field;
        }

        public void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record)
        {
            writer.WriteField(field.GetDisplayValue(record));
        }

        public void WriteHeader(ISpreadsheetWriter writer)
        {
            writer.WriteField(header);
        }
    }
}