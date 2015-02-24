using SqlImport;

namespace CsvExport
{
    internal class NullCsvColumn : ICsvColumn
    {
        public void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record)
        {            
        }

        public void WriteHeader(ISpreadsheetWriter writer)
        {            
        }
    }
}