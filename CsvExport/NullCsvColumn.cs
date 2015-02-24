using Model;
using SqlImport;

namespace CsvExport
{
    public class NullCsvColumn : ICsvColumn
    {
        public void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record)
        {            
        }

        public void WriteHeader(ISpreadsheetWriter writer)
        {            
        }
    }
}