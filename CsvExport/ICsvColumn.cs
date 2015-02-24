using Model;
using SqlImport;

namespace CsvExport
{
    public interface ICsvColumn
    {
        void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record);
        void WriteHeader(ISpreadsheetWriter writer);
    }
}