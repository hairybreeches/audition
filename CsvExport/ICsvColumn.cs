using SqlImport;

namespace CsvExport
{
    internal interface ICsvColumn
    {
        void WriteField(ISpreadsheetWriter writer, SqlLedgerEntry record);
        void WriteHeader(ISpreadsheetWriter writer);
    }
}