using System;
using Capabilities;
using Model;
using SqlImport;

namespace CsvExport
{
    public class CsvColumn : ICsvColumn
    {
        private readonly DisplayField field;

        public CsvColumn(DisplayField field)
        {
            this.field = field;
        }

        public void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record)
        {
            writer.WriteField(field.GetDisplayValue(record));
        }

        public void WriteHeader(ISpreadsheetWriter writer)
        {
            writer.WriteField(field.GetHeaderValue());
        }
    }
}