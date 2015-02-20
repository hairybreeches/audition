namespace CsvExport
{
    internal interface ICsvColumn<in TRecord>
    {
        void WriteField(ISpreadsheetWriter writer, TRecord record);
        void WriteHeader(ISpreadsheetWriter writer);
    }
}